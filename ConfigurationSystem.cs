using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
	public class ConfigurationSystem : ModSystem
	{
		//These assume no ILoadable across the whole mod has a duplicate name (contrary to what tml allows)
		public static Dictionary<string, ContentType> NonLoadedNames { get; private set; }
		public static Dictionary<ContentType, List<string>> NonLoadedNamesByType { get; private set; }

        public override void OnModLoad()
        {
			NonLoadedNames = new();
			NonLoadedNamesByType = new();

			//Debugging only
			var autoloadedContent = Mod.GetContent().ToList();
			var manuallyAddedTypes = new List<Type>();

			Type modType = Mod.GetType();
			foreach (Type type in Mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
			{
				//Mirror autoloading conditions
				if (type == modType) continue;
				if (type.IsAbstract) continue;
				if (type.ContainsGenericParameters) continue;
				if (type.GetConstructor(Array.Empty<Type>()) == null) continue; //Don't autoload things with no default constructor
				if (!typeof(ILoadable).IsAssignableFrom(type)) continue; //Don't autoload non-ILoadables

				var autoload = AutoloadAttribute.GetValue(type);

				if (autoload.NeedsAutoloading) continue; //Skip things that are autoloaded (this code runs after Autoload())

				var content = ContentAttribute.GetValue(type);

				if (content.Ignore) continue; //Skip things tagged as non-autoloadable, yet shouldn't be loaded through here (loaded elsewhere)

				var reason = FindContentFilterReason(content.ContentType);
				var instance = (ILoadable)Activator.CreateInstance(type);

				if (reason == ContentType.Always)
				{
					//No filters
					manuallyAddedTypes.Add(type);
					Mod.AddContent(instance);
					continue; //Don't do anything further
				}

				if (instance is ModType modTypeInstance)
                {
                    string name = modTypeInstance.Name;
                    NonLoadedNames.Add(name, reason);

					if (!NonLoadedNamesByType.ContainsKey(reason))
                    {
						NonLoadedNamesByType[reason] = new List<string>();
					}

                    NonLoadedNamesByType[reason].Add(name);
				}
			}
		}

        private static ContentType FindContentFilterReason(ContentType contentType)
        {
            //If atleast one toggle is false, and the content type matches that toggle, return that content type

            const ContentType always = ContentType.Always;
            if (contentType == always)
            {
				//Skip checking if this is not filtered anyway
				return always;
			}

			var config = AConfigurationConfig.Instance;

			if (!config.Bosses && contentType.HasFlag(ContentType.Bosses))
            {
				return ContentType.Bosses;
			}
			if (!config.HostileNPCs && contentType.HasFlag(ContentType.HostileNPCs))
			{
				return ContentType.HostileNPCs;
			}
			if (!config.FriendlyNPCs && contentType.HasFlag(ContentType.FriendlyNPCs))
			{
				return ContentType.FriendlyNPCs;
			}
			if (!config.BossConsolation && contentType.HasFlag(ContentType.BossConsolation))
			{
				return ContentType.BossConsolation;
			}

			//No filters, ignore
			return always;
		}

        public override void Unload()
        {
			NonLoadedNames?.Clear();
			NonLoadedNames = null;

			NonLoadedNamesByType?.Clear();
			NonLoadedNamesByType = null;
		}

		public static string ContentTypeToString(ContentType contentType)
        {
            return contentType switch
            {
                ContentType.Always => string.Empty,
                ContentType.Bosses => "Bosses",
                ContentType.HostileNPCs => "Hostile NPCs",
                ContentType.FriendlyNPCs => "Friendly NPCs",
				ContentType.BossConsolation => "Boss Consolation Items",
				_ => string.Empty,
            };
		}
	}

	[Flags]
	public enum ContentType : byte
    {
		Always = 0 << 0,
		Bosses = 1 << 1,
		HostileNPCs = 1 << 2,
		FriendlyNPCs = 1 << 3,
		BossConsolation = 1 << 4,
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class ContentAttribute : Attribute
	{
		private static readonly ContentAttribute Default = new ContentAttribute(ContentType.Always);

		public ContentType ContentType { get; private set; }

		public bool Ignore { get; private set; }

		public ContentAttribute(ContentType contentType, bool ignore = false)
		{
			ContentType = contentType;
			Ignore = ignore;
		}

		public static ContentAttribute GetValue(Type type)
		{
			//Get all attributes on the type.
			object[] all = type.GetCustomAttributes(typeof(ContentAttribute), true);
			//The first should be the most derived attribute.
			var mostDerived = (ContentAttribute)all.FirstOrDefault();
			//If there were no declarations, then return default.
			return mostDerived ?? Default;
		}
	}
}
