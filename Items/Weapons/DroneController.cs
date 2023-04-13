using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.Drones;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class DroneController : MinionItemBase
	{
		#region Static Methods
		/// <summary>
		/// Checks if drones are unlocked 
		/// </summary>
		public static bool AllUnlocked(DroneType unlocked)
		{
			foreach (DroneType type in Enum.GetValues(typeof(DroneType)))
			{
				if (type != DroneType.None)
				{
					if (!unlocked.HasFlag(type))
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Checks if all drones are unlocked for the player
		/// </summary>
		public static bool AllUnlocked(Player player)
		{
			return AllUnlocked(player.GetModPlayer<AssPlayer>().droneControllerUnlocked);
		}

		/// <summary>
		/// Returns the custom MinionPos
		/// </summary>
		public static int GetSlotOfNextDrone(Projectile self)
		{
			int slot = 0;
			int min = Main.maxProjectiles;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active)
				{
					if (proj.owner == self.owner && proj.identity != self.identity)
					{
						if (proj.ModProjectile is DroneBase drone && drone.IsCombatDrone)
						{
							int minionPos = drone.MinionPos;
							min = Math.Min(min, minionPos);
							if (minionPos > slot) slot = minionPos;
						}
					}
				}
			}
			if (min > 0) return 0;

			return slot + 1;
		}

		/// <summary>
		/// Determines position for the current projectile based on minionPos
		/// </summary>
		public static Vector2 GetPosition(Projectile projectile, int minionPos)
		{
			int initialVecticalOffset = -70;
			int initialHorizontalOffset = 0;
			int verticalOffset = -(int)(projectile.height * 1.2f);
			int horizontalOffset = (int)(projectile.width * 1.75f);
			int tempPos = minionPos % 6;
			int side = minionPos % 2; //1 means left, 0 means right
			int columnOffset = minionPos / 6 + 1; //1 for <6, 2 for <12, etc

			int rowOffset = tempPos / 2; //0, 1 or 2

			int middleOffset = 0;
			if (rowOffset == 1) middleOffset = horizontalOffset / 2; //the middle one

			Vector2 offset = new Vector2
				(
				initialHorizontalOffset + columnOffset * horizontalOffset + middleOffset,
				initialVecticalOffset + rowOffset * verticalOffset
				);
			offset.X *= side == 1 ? -1 : 1;
			return offset;
		}

		public static int SumOfSummonedDrones(Player player)
		{
			int sum = 0;
			for (int i = 0; i < DataList.Length; i++)
			{
				sum += player.ownedProjectileCounts[DataList[i].ProjType];
			}
			return sum;
		}

		public static bool CanSpawn(Player player, DroneType selected, out bool blocked)
		{
			bool canSpawn = true;
			blocked = false;
			if (selected == DroneType.Healing || selected == DroneType.Shield)
			{
				canSpawn = player.ownedProjectileCounts[GetDroneData(selected).ProjType] == 0;
				if (!canSpawn) blocked = true;
			}
			canSpawn &= player.GetModPlayer<AssPlayer>().droneControllerUnlocked.HasFlag(selected);
			return canSpawn;
		}

		/// <summary>
		/// Sets the data up for a DroneType
		/// </summary>
		public static DroneData SetDroneData(DroneType selected)
		{
			switch (selected)
			{
				case DroneType.BasicLaser:
					return new DroneData
						(
						projType: ModContent.ProjectileType<BasicLaserDrone>(),
						droneType: selected
						);
				case DroneType.HeavyLaser:
					return new DroneData
						(
						projType: ModContent.ProjectileType<HeavyLaserDrone>(),
						droneType: selected,
						dmgModifier: 8.091f,
						kBModifier: 4f
						);
				case DroneType.Missile:
					return new DroneData
						(
						projType: ModContent.ProjectileType<MissileDrone>(),
						droneType: selected,
						dmgModifier: 2.19f,
						kBModifier: 1.2f
						);
				case DroneType.Healing:
					return new DroneData
						(
						projType: ModContent.ProjectileType<HealingDrone>(),
						droneType: selected,
						combat: false
						);
				case DroneType.Shield:
					return new DroneData
						(
						projType: ModContent.ProjectileType<ShieldDrone>(),
						droneType: selected,
						combat: false
						);
				default:
					throw new Exception("No DroneType specified");
			}
		}

		/// <summary>
		/// Holds data about each DroneType
		/// </summary>
		public static DroneData[] DataList;

		public static LocalizedText BaseDamageText { get; private set; }

		public static LocalizedText BaseKnockbackText { get; private set; }

		/// <summary>
		/// Used to access a particular DroneTypes data
		/// </summary>
		public static DroneData GetDroneData(DroneType selected)
		{
			return DataList[(int)Math.Log((int)selected, 2)];
		}

		public static CircleUIConf GetUIConf()
		{
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			List<string> tooltips = new List<string>();
			List<string> toUnlock = new List<string>();
			List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>();
			List<bool> unlocked = new List<bool>();

			foreach (DroneType type in Enum.GetValues(typeof(DroneType)))
			{
				if (type != DroneType.None)
				{
					DroneData data = GetDroneData(type);
					assets.Add(AssUtils.Instance.Assets.Request<Texture2D>(data.PreviewTextureName));
					unlocked.Add(mPlayer.droneControllerUnlocked.HasFlag(type));
					tooltips.Add(data.UITooltip);
					toUnlock.Add(ToUnlockText.Format(data.ComponentName.ToString()));
				}
			}

			return new CircleUIConf(0, -1, assets, unlocked, tooltips, toUnlock);
		}

		/// <summary>
		/// Called in Mod.Load
		/// </summary>
		public static void DoLoad()
		{
			if (!ContentConfig.Instance.Weapons)
			{
				return;
			}

			string category = $"{nameof(DroneData)}.";
			BaseDamageText = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{category}BaseDamage"));
			BaseKnockbackText = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{category}BaseKnockback"));

			Array a = Enum.GetValues(typeof(DroneType));
			DataList = new DroneData[a.Length - 1]; //without None
			int i = 0;
			foreach (DroneType type in a)
			{
				if (type != DroneType.None)
				{
					DataList[i++] = SetDroneData(type);
				}
			}
		}

		public override void Unload()
		{
			DataList = null;
		}
		#endregion

		public const int BaseDmg = 22;
		public const float BaseKB = 2.5f;

		public static LocalizedText FirerateText { get; private set; }
		public static LocalizedText UnlockMoreText { get; private set; }
		public static LocalizedText CanSpawnText { get; private set; }
		public static LocalizedText ToUnlockText { get; private set; }

		public override void EvenSaferSetStaticDefaults()
		{
			FirerateText = this.GetLocalization("Firerate");
			UnlockMoreText = this.GetLocalization("UnlockMore");
			CanSpawnText = this.GetLocalization("CanSpawn");
			ToUnlockText = this.GetLocalization("ToUnlock");
		}

		public override void SetDefaults()
		{
			Item.damage = BaseDmg;
			Item.knockBack = BaseKB;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = 4;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ModContent.ProjectileType<BasicLaserDrone>();
			Item.shootSpeed = 10f;
			Item.buffType = ModContent.BuffType<DroneControllerBuff>();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			DroneType selected = mPlayer.selectedDroneControllerMinionType;
			damage += GetDroneData(selected).DmgModifier;
		}

		public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
		{
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			DroneType selected = mPlayer.selectedDroneControllerMinionType;
			knockback *= GetDroneData(selected).KBModifier;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
			{
				AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
				DroneType selected = mPlayer.selectedDroneControllerMinionType;

				if (!CanSpawn(player, selected, out _))
				{
					return false;
				}
			}
			return true;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
			DroneType selected = mPlayer.selectedDroneControllerMinionType;
			DroneData droneData = GetDroneData(selected);
			type = droneData.ProjType;

			int index = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, player.velocity.Y - 6f, type, damage, knockback, Main.myPlayer);
			Main.projectile[index].originalDamage = (int)(Item.damage * (1f + droneData.DmgModifier));
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 1).AddIngredient(ItemID.Switch, 2).AddIngredient(ItemID.Wire, 10).AddTile(TileID.MythrilAnvil).Register();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			DroneType selected = mPlayer.selectedDroneControllerMinionType;

			DroneData data = GetDroneData(selected);

			int damageIndex = -1;
			int knockbackIndex = -1;

			bool hasController = Main.LocalPlayer.HasItem(Item.type);

			for (int i = 0; i < tooltips.Count; i++)
			{
				TooltipLine line = tooltips[i];
				if (hasController)
				{
					if (line.Mod == "Terraria" && line.Name == "ItemName")
					{
						line.Text += " (" + data.NameSingular + ")";
					}
				}

				if (line.Mod == "Terraria" && line.Name == "Damage")
				{
					damageIndex = i;
				}

				if (line.Mod == "Terraria" && line.Name == "Knockback")
				{
					knockbackIndex = i;
				}
			}

			if (damageIndex > -1)
			{
				if (!data.Combat)
				{
					tooltips.RemoveAt(damageIndex);
				}
			}

			if (knockbackIndex != -1)
			{
				if (data.Combat)
				{
					if (data.Firerate.ToString() != "") tooltips.Insert(knockbackIndex, new TooltipLine(Mod, nameof(FirerateText), FirerateText.Format(data.Firerate)));
				}
				else
				{
					//here damageIndex one is removed, so find knockbackindex again
					knockbackIndex = tooltips.FindIndex(line => line.Name == "Knockback");
					if (knockbackIndex > -1) tooltips.RemoveAt(knockbackIndex);
				}
			}

			bool allUnlocked = AllUnlocked(mPlayer.droneControllerUnlocked);

			if (!(allUnlocked && hasController))
			{
				tooltips.Add(new TooltipLine(Mod, nameof(UnlockMoreText), UnlockMoreText.ToString()));
			}

			CanSpawn(Main.LocalPlayer, selected, out bool blocked);
			if (hasController && blocked)
			{
				tooltips.Add(new TooltipLine(Mod, nameof(CanSpawnText), CanSpawnText.Format(data.NameSingular)));
			}
		}
	}

	/// <summary>
	/// Holds data about a DroneType
	/// </summary>
	public struct DroneData
	{
		public readonly int ProjType;
		public readonly string InternalName;
		public readonly LocalizedText Name;
		public readonly LocalizedText ComponentName;
		public readonly LocalizedText Firerate;
		public readonly LocalizedText Description;
		public readonly LocalizedText Misc;
		public readonly float DmgModifier;
		public readonly float KBModifier;
		public readonly bool Combat;

		public string NameSingular => Name.Format(1);
		public string UITooltip => NameSingular
			+ (Combat ? ($"\n{DroneController.BaseDamageText.Format((int)(DroneController.BaseDmg * (DmgModifier + 1f)))}"
			+ $"\n{DroneController.BaseKnockbackText.Format(Math.Round(DroneController.BaseKB * KBModifier, 1))}") : "")
			+ "\n" + Description.ToString()
			+ "\n" + Misc.ToString();

		public string PreviewTextureName => $"Projectiles/Minions/Drones/{InternalName}Preview"; 

		public DroneData(int projType, DroneType droneType, float dmgModifier = 0f, float kBModifier = 1f, bool combat = true)
		{
			ProjType = projType;
			InternalName = GetInternalName(droneType);
			string thisKey = $"DroneData.{InternalName}.";
			Name = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}DisplayName"), () => "");
			ComponentName = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}ComponentName"), () => "");
			Firerate = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}Firerate"), () => "");
			Description = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}Description"), () => "");
			Misc = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}Misc"), () => "");
			DmgModifier = dmgModifier;
			KBModifier = kBModifier;
			Combat = combat;
		}

		public static string GetInternalName(DroneType droneType)
		{
			return droneType switch
			{
				DroneType.BasicLaser => "BasicLaserDrone",
				DroneType.HeavyLaser => "HeavyLaserDrone",
				DroneType.Missile => "MissileDrone",
				DroneType.Healing => "HealingDrone",
				DroneType.Shield => "ShieldDrone",
				_ => throw new Exception("No DroneType specified"),
			};
		}
	}

	/// <summary>
	/// The type of drone enumerated, so you can check against it via .HasFlag(DroneType.SomeType)
	/// </summary>
	[Flags]
	public enum DroneType : byte
	{
		None = 0,
		BasicLaser = 1 << 0,
		HeavyLaser = 1 << 1,
		Missile = 1 << 2,
		Healing = 1 << 3,
		Shield = 1 << 4,
	}
}
