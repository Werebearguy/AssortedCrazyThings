using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using AssortedCrazyThings.Base;

namespace AssortedCrazyThings
{
	public static class ConfigurationSystem //Cannot be a ModSystem since that load runs post-mod.Load, we need it as early as possible (but after Autoload has taken place, so ILoadable.Load won't work either)
	{
		//These assume no ILoadable across the whole mod has a duplicate name (contrary to what tml allows)
		public static Dictionary<string, ContentType> NonLoadedNames { get; private set; }
		public static Dictionary<ContentType, List<string>> NonLoadedNamesByType { get; private set; }

        public static void Load()
        {
			Mod mod = AssUtils.Instance; //Maybe change it to support all loaded mod assemblies
			NonLoadedNames = new();
			NonLoadedNamesByType = new();

			//Debugging only
			var autoloadedContent = mod.GetContent().ToList();
			var manuallyAddedTypes = new List<Type>();

			Type modType = mod.GetType();
			foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
			{
				//Mirror autoloading conditions
				if (type == modType) continue;
				if (type.IsAbstract) continue;
				if (type.ContainsGenericParameters) continue;
				if (type.GetConstructor(Array.Empty<Type>()) == null) continue; //Don't autoload things with no default constructor
				if (!typeof(ILoadable).IsAssignableFrom(type)) continue; //Don't autoload non-ILoadables

				var autoload = AutoloadAttribute.GetValue(type);

				if (autoload.NeedsAutoloading) continue; //Skip things that are autoloaded (this code runs after Autoload())

				if (!LoadSide(autoload.Side)) continue; //Skip things that shouldn't load on a particular side

				var content = ContentAttribute.GetValue(type);

				if (content.Ignore) continue; //Skip things tagged as non-autoloadable, yet shouldn't be loaded through here (loaded elsewhere)

                var reasons = FindContentFilterReasons(content.ContentType);
				var instance = (ILoadable)Activator.CreateInstance(type);

				if (reasons == ContentType.Always)
				{
					//No filters
					manuallyAddedTypes.Add(type);
					mod.AddContent(instance);
					continue; //Don't do anything further
				}

				if (instance is ModType modTypeInstance)
                {
                    string name = modTypeInstance.Name;
                    NonLoadedNames.Add(name, reasons);

					if (!NonLoadedNamesByType.ContainsKey(reasons))
                    {
						NonLoadedNamesByType[reasons] = new List<string>();
					}

                    NonLoadedNamesByType[reasons].Add(name);
				}
			}

			int b = 0;
		}

        private static ContentType FindContentFilterReasons(ContentType contentType)
        {
			//Bitwise "and" results in the overlap, representing the flags that caused the content to be filtered
			return AConfigurationConfig.Instance.FilterFlags & contentType;
		}

        public static void Unload()
        {
			NonLoadedNames?.Clear();
			NonLoadedNames = null;

			NonLoadedNamesByType?.Clear();
			NonLoadedNamesByType = null;
		}

		public static string ContentTypeToString(ContentType contentType)
        {
			if (!ExactlyOneFlagSet(contentType))
            {
				string concat = string.Empty;
				foreach (ContentType flag in Enum.GetValues(typeof(ContentType)))
                {
					if (flag != ContentType.Always && contentType.HasFlag(flag))
                    {
						concat += ContentTypeToString(flag) + "/";
					}
                }
				return concat[0..^1];
			}

            return contentType switch
            {
                ContentType.Always => string.Empty,
                ContentType.Bosses => "Bosses",
                ContentType.HostileNPCs => "Hostile NPCs",
                ContentType.FriendlyNPCs => "Friendly NPCs",
				ContentType.CuteSlimes => "Cute Slimes",
				ContentType.DroppedPets => "Dropped Pets",
				ContentType.OtherPets => "Other Pets",
				ContentType.Weapons => "Weapons",
				ContentType.Tools => "Tools",
				ContentType.Placeables => "Placeables",
				ContentType.Armor => "Armor",
				ContentType.VanityArmor => "Vanity Armor",
				ContentType.Accessories => "Accessories",
				ContentType.VanityAccessories => "Vanity Accessories",
				ContentType.BossConsolation => "Boss Consolation Items",
				_ => string.Empty,
            };
		}

		public static bool ExactlyOneFlagSet(ContentType contentType)
		{
			return Enum.IsDefined(typeof(ContentType), contentType);
		}

		//Copied from tmodloader since it's internal
		public static bool LoadSide(ModSide side)
		{
			return side != (Main.dedServ ? ModSide.Client : ModSide.Server);
		}
	}

	[Flags]
	public enum ContentType : short
    {
		Always = 0 << 0,
		Bosses = 1 << 1,
		HostileNPCs = 1 << 2,
		FriendlyNPCs = 1 << 3,
		CuteSlimes = 1 << 4,
		DroppedPets = 1 << 5,
		OtherPets = 1 << 6,
		Weapons = 1 << 7,
		Tools = 1 << 8,
		Placeables = 1 << 9,
		Armor = 1 << 10,
		VanityArmor = 1 << 11,
		Accessories = 1 << 12,
		VanityAccessories = 1 << 13,
		BossConsolation = 1 << 14,
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
