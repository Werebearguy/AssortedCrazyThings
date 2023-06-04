using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
	public static class ConfigurationSystem //Cannot be a ModSystem since that load runs post-mod.Load, we need it as early as possible (but after Autoload has taken place, so ILoadable.Load won't work either)
	{
		//These assume no ILoadable across the whole mod has a duplicate name (contrary to what tml allows)
		public static Dictionary<string, ContentType> NonLoadedNames { get; private set; }
		public static Dictionary<ContentType, List<string>> NonLoadedNamesByType { get; private set; }

		private static Type[] InvalidTypes { get; set; }

		/// <summary>
		/// For use on generic content that should be always loaded except when everything is disabled
		/// </summary>
		public const ContentType AllFlags =
			ContentType.Bosses |
			ContentType.CuteSlimes |
			ContentType.HostileNPCs |
			ContentType.FriendlyNPCs |
			ContentType.DroppedPets |
			ContentType.OtherPets |
			ContentType.Weapons |
			ContentType.Tools |
			ContentType.PlaceablesFunctional |
			ContentType.PlaceablesDecorative |
			ContentType.Armor |
			ContentType.VanityArmor |
			ContentType.Accessories |
			ContentType.VanityAccessories |
			ContentType.BossConsolation |
			ContentType.AommSupport;

		public static void Load()
		{
			Mod mod = AssUtils.Instance; //Maybe change it to support all loaded mod assemblies
			NonLoadedNames = new();
			NonLoadedNamesByType = new();

			InvalidTypes = GetInvalidTypes();

			//Debugging only
			var autoloadedContent = mod.GetContent().ToList();
			//SimpleModGore gets loaded. Maybe disable gore loading?
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

				CheckInvalidInheritance(type);

				if (autoload.NeedsAutoloading) continue; //Skip things that are autoloaded (this code runs after Autoload())

				if (!LoadSide(autoload.Side)) continue; //Skip things that shouldn't load on a particular side

				var content = ContentAttribute.GetValue(type);

				if (content.Ignore) continue; //Skip things tagged as non-autoloadable, yet shouldn't be loaded through here (loaded elsewhere)

				var reasons = FindContentFilterReasons(content);
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

		private static Type[] GetInvalidTypes()
		{
			return new Type[]
			{
				typeof(ModItem),
				typeof(ModProjectile),
				typeof(ModBuff),
				typeof(ModMount),
				typeof(ModNPC),
				typeof(ModTile),
				typeof(PlayerDrawLayer),
				typeof(ModPlayer),
				typeof(ModSystem),
				typeof(GlobalNPC),
				typeof(GlobalBuff),
				typeof(GlobalProjectile),
				typeof(GlobalItem),
				typeof(GlobalTile),
				typeof(GlobalWall),
			};
		}

		private static void CheckInvalidInheritance(Type type)
		{
			//Detect misuse of tml types that have an existing Ass base class
			if (!typeof(ILoadable).IsAssignableFrom(type))
			{
				return;
			}

			var baseType = type.BaseType;

			if (baseType != null && Array.IndexOf(InvalidTypes, baseType) > -1)
			{
				throw new Exception($"{type} inherits from {baseType}, which is not permitted. Use the base classes in ConfigurationBaseClasses.cs");
			}
		}

		private static ContentType FindContentFilterReasons(ContentAttribute content)
		{
			//Bitwise "and" results in the overlap, representing the flags that caused the content to be filtered
			var flags = ContentConfig.Instance.FilterFlags & content.ContentType;

			if (content.NeedsAllToFilterOut && flags != content.ContentType)
			{
				//If the content needs a full match
				return ContentType.Always;
			}

			return flags;
		}

		public static void Unload()
		{
			NonLoadedNames?.Clear();
			NonLoadedNames = null;

			NonLoadedNamesByType?.Clear();
			NonLoadedNamesByType = null;

			InvalidTypes = null;
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
				ContentType.CuteSlimes => "Cute Slimes",
				ContentType.HostileNPCs => "Hostile NPCs",
				ContentType.FriendlyNPCs => "Friendly NPCs",
				ContentType.DroppedPets => "Dropped Pets",
				ContentType.OtherPets => "Other Pets",
				ContentType.Weapons => "Weapons",
				ContentType.Tools => "Tools",
				ContentType.PlaceablesFunctional => "Placeables (functional)",
				ContentType.PlaceablesDecorative => "Placeables (decorative)",
				ContentType.Armor => "Armor",
				ContentType.VanityArmor => "Vanity Armor",
				ContentType.Accessories => "Accessories",
				ContentType.VanityAccessories => "Vanity Accessories",
				ContentType.BossConsolation => "Boss Consolation Items",
				ContentType.AommSupport => "'The Amulet Of Many Minions' content",
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
	public enum ContentType : int
	{
		Always = 0 << 0,
		Bosses = 1 << 1,
		CuteSlimes = 1 << 2,
		HostileNPCs = 1 << 3,
		FriendlyNPCs = 1 << 4,
		DroppedPets = 1 << 5,
		OtherPets = 1 << 6,
		Weapons = 1 << 7,
		Tools = 1 << 8,
		PlaceablesFunctional = 1 << 9,
		PlaceablesDecorative = 1 << 10,
		Armor = 1 << 11,
		VanityArmor = 1 << 12,
		Accessories = 1 << 13,
		VanityAccessories = 1 << 14,
		BossConsolation = 1 << 15,
		AommSupport = 1 << 16,
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class ContentAttribute : Attribute
	{
		private static readonly ContentAttribute Default = new ContentAttribute(ContentType.Always);

		public ContentType ContentType { get; private set; }

		public bool NeedsAllToFilterOut { get; private set; }

		public bool Ignore { get; private set; }

		public ContentAttribute(ContentType contentType, bool needsAllToFilterOut = false, bool ignore = false)
		{
			ContentType = contentType;
			NeedsAllToFilterOut = needsAllToFilterOut;
			if (ContentType == ConfigurationSystem.AllFlags)
			{
				NeedsAllToFilterOut = true; //The opposite makes no sense
			}
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
