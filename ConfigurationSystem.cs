using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
	public class ConfigurationSystem : ModSystem
    {
		public static List<string> NonLoadedNames;

        public override void OnModLoad()
        {
			NonLoadedNames = new List<string>();

			AConfigurationConfig config = ModContent.GetInstance<AConfigurationConfig>();

			List<Type> manuallyAddedTypes = new List<Type>();

			Type modType = Mod.GetType();
			foreach (Type type in Mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
			{
				//Mirror autoloading conditions
				if (type == modType) continue;
				if (type.IsAbstract) continue;
				if (type.ContainsGenericParameters) continue;
				if (type.GetConstructor(Array.Empty<Type>()) == null) continue;//don't autoload things with no default constructor

				if (typeof(ILoadable).IsAssignableFrom(type))
				{
					var autoload = AutoloadAttribute.GetValue(type);

					if (!autoload.NeedsAutoloading) //Only manually add types that have autoloading disabled
					{
						var content = ContentAttribute.GetValue(type);
						ILoadable instance = (ILoadable)Activator.CreateInstance(type);

						//TODO proper filter here
						//If atleast one flag is matching a true config, autoload it
						if (config.Bosses && content.ContentType.HasFlag(ContentType.Boss))
						{
							manuallyAddedTypes.Add(type);
                            Mod.AddContent(instance);
						}
                        else
                        {
							if (instance is ModType modTypeInstance)
                            {
								NonLoadedNames.Add(modTypeInstance.Name);
							}
						}
					}
				}
			}
		}

        public override void Unload()
        {
			NonLoadedNames?.Clear();
			NonLoadedNames = null;
		}
    }

	[Flags]
	public enum ContentType : byte
    {
		Always = 0 << 0,
		NPC = 1 << 0,
		Boss = 1 << 1,
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class ContentAttribute : Attribute
	{
		private static readonly ContentAttribute Default = new ContentAttribute(ContentType.Always);

		public ContentType ContentType { get; private set; }

		public ContentAttribute(ContentType contentType)
        {
			ContentType = contentType;
		}

		public static ContentAttribute GetValue(Type type)
		{
			//Get all attributes on the type.
			object[] all = type.GetCustomAttributes(typeof(ContentAttribute), true);
			//The first should be the most derived attribute.
			var mostDerived = (ContentAttribute)all.FirstOrDefault();
			//If there were no declarations, then return null.
			return mostDerived ?? Default;
		}
	}
}
