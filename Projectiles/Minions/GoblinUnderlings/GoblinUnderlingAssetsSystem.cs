using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	public enum GoblinUnderlingWeaponType
	{
		Sword,
		Bow
	}

	[Content(ContentType.Weapons)]
	[Autoload(Side = ModSide.Client)]
	public class GoblinUnderlingAssetsSystem : AssSystem
	{
		public static Dictionary<int, Asset<Texture2D>[]> BodyAssets { get; private set; }
		private static Dictionary<GoblinUnderlingWeaponType, Asset<Texture2D>[]> WeaponAssets { get; set; }
		private static Dictionary<GoblinUnderlingWeaponType, List<int>> HasNoWeaponAssets { get; set; }
		public static Dictionary<int, string> AssetPrefixes { get; private set; }

		/// <summary>
		/// Can return null if no weapon exists for the given type and index
		/// </summary>
		public static Asset<Texture2D> GetWeaponAsset(GoblinUnderlingWeaponType weaponType, int index)
		{
			if (HasNoWeaponAssets[weaponType].Contains(index))
			{
				return null;
			}

			return WeaponAssets[weaponType][index];
		}

		public static void RegisterAssetPrefix(int type, string prefix)
		{
			if (Main.dedServ)
			{
				return;
			}

			AssetPrefixes[type] = prefix;
		}

		//Has to be after tiers are assigned
		private static void LoadTextures()
		{
			var count = GoblinUnderlingTierSystem.TierCount;
			BodyAssets = new Dictionary<int, Asset<Texture2D>[]>();
			WeaponAssets = new Dictionary<GoblinUnderlingWeaponType, Asset<Texture2D>[]>();
			HasNoWeaponAssets = new Dictionary<GoblinUnderlingWeaponType, List<int>>();

			var tiers = GoblinUnderlingTierSystem.GetTiers();
			foreach (var pair in AssetPrefixes)
			{
				int type = pair.Key;
				BodyAssets[type] = new Asset<Texture2D>[count];
				foreach (var tier in tiers)
				{
					int index = (int)tier;
					string assetPrefix = pair.Value;
					BodyAssets[type][index] = ModContent.Request<Texture2D>(assetPrefix + "_" + index);
				}
			}

			//Uses special ranged projectile
			HasNoWeaponAssets[GoblinUnderlingWeaponType.Sword] = new List<int>() { (int)GoblinUnderlingProgressionTierStage.Cultist };

			string weaponAssetPrefix = "AssortedCrazyThings/Projectiles/Minions/GoblinUnderlings/Weapons/Weapon";
			//TODO goblin fix when all weapon types are in
			if (GoblinUnderlingWeaponType.Sword is GoblinUnderlingWeaponType weaponType)
			//foreach (var weaponType in Enum.GetValues<GoblinUnderlingWeaponType>())
			{
				WeaponAssets[weaponType] = new Asset<Texture2D>[count];
				foreach (var tier in tiers)
				{
					int index = (int)tier;
					if (HasNoWeaponAssets[weaponType].Contains(index))
					{
						continue;
					}
					WeaponAssets[weaponType][index] = ModContent.Request<Texture2D>(weaponAssetPrefix + weaponType.ToString() + "_" + index);
				}
			}
		}

		public override void OnModLoad()
		{
			AssetPrefixes = new();
		}

		public override void PostSetupContent()
		{
			LoadTextures();
		}

		public override void Unload()
		{
			AssetPrefixes = null;

			BodyAssets = null;
			WeaponAssets = null;
		}
	}
}
