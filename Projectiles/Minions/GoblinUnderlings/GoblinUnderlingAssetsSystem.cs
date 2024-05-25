using AssortedCrazyThings.Base.Handlers.ProgressionTierHandler;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
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
	[Autoload(false, Side = ModSide.Client)]
	public class GoblinUnderlingAssetsSystem : AssSystem
	{
		public static readonly string assetPath = "AssortedCrazyThings/Projectiles/Minions/GoblinUnderlings";

		public static Dictionary<GoblinUnderlingClass, int> BodyAssetFrameCounts { get; private set; }
		public static Dictionary<int, Dictionary<GoblinUnderlingClass, Asset<Texture2D>[]>> BodyAssets { get; private set; }
		public static Dictionary<int, Asset<Texture2D>[]> RangedArmAssets { get; private set; }
		private static Dictionary<GoblinUnderlingWeaponType, Asset<Texture2D>[]> WeaponAssets { get; set; }
		private static Dictionary<GoblinUnderlingWeaponType, List<int>> HasNoWeaponAssets { get; set; }
		public static Dictionary<int, string> AssetPrefixes { get; private set; }
		public static Asset<Texture2D> RocketBootsAsset { get; private set; }

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

		public static void SetFrameCount(int type, GoblinUnderlingClass @class)
		{
			if (Main.dedServ)
			{
				return;
			}

			Main.projFrames[type] = BodyAssetFrameCounts[@class];
		}

		//Has to be after tiers are assigned
		private static void LoadTextures()
		{
			//If some tiers aren't defined, the assets will be null, but it should never be fetched anyway
			var count = ProgressionTierSystem.GlobalTierCount;
			BodyAssets = new Dictionary<int, Dictionary<GoblinUnderlingClass, Asset<Texture2D>[]>>();
			RangedArmAssets = new Dictionary<int, Asset<Texture2D>[]>();
			WeaponAssets = new Dictionary<GoblinUnderlingWeaponType, Asset<Texture2D>[]>();
			RocketBootsAsset = ModContent.Request<Texture2D>($"{assetPath}/RocketBoots");
			HasNoWeaponAssets = new Dictionary<GoblinUnderlingWeaponType, List<int>>();

			var tiers = GoblinUnderlingTierSystem.GetTiers();
			foreach (var pair in AssetPrefixes)
			{
				int type = pair.Key;
				BodyAssets[type] = new Dictionary<GoblinUnderlingClass, Asset<Texture2D>[]>();
				RangedArmAssets[type] = new Asset<Texture2D>[count];
				foreach (var @class in Enum.GetValues<GoblinUnderlingClass>())
				{
					BodyAssets[type][@class] = new Asset<Texture2D>[count];
					foreach (var tier in tiers)
					{
						int index = (int)tier;
						string assetPrefix = pair.Value;
						BodyAssets[type][@class][index] = ModContent.Request<Texture2D>(assetPrefix + @class + "_" + index);

						if (@class == GoblinUnderlingClass.Ranged)
						{
							RangedArmAssets[type][index] = ModContent.Request<Texture2D>(assetPrefix + @class + "Arm_" + index);
						}
					}
				}
			}

			//Uses special ranged projectile
			HasNoWeaponAssets[GoblinUnderlingWeaponType.Sword] = new List<int>() { (int)ProgressionTierStage.Cultist };
			//Uses arm sprite
			HasNoWeaponAssets[GoblinUnderlingWeaponType.Bow] = new List<int>() { (int)ProgressionTierStage.Cultist };

			string weaponAssetPrefix = "AssortedCrazyThings/Projectiles/Minions/GoblinUnderlings/Weapons/Weapon";
			foreach (var weaponType in Enum.GetValues<GoblinUnderlingWeaponType>())
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

			BodyAssetFrameCounts = new();
			BodyAssetFrameCounts[GoblinUnderlingClass.Melee] = 20;
			BodyAssetFrameCounts[GoblinUnderlingClass.Magic] = 14;
			BodyAssetFrameCounts[GoblinUnderlingClass.Ranged] = 14;
		}

		public override void PostSetupContent()
		{
			LoadTextures();
		}

		public override void Unload()
		{
			AssetPrefixes = null;

			BodyAssetFrameCounts = null;
			BodyAssets = null;
			RangedArmAssets = null;
			WeaponAssets = null;
			RocketBootsAsset = null;
		}
	}
}
