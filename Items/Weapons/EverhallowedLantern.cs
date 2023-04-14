using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Items.Armor;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{

	/// <summary>
	/// Holds the type of Dungeon Soul (All just for Everhallowed Lantern)
	/// </summary>
	[Flags]
	public enum SoulType : byte
	{
		None = 0,
		Dungeon = 1 << 0,
		Fright = 1 << 1,
		Sight = 1 << 2,
		Might = 1 << 3,
	}

	/// <summary>
	/// Holds data about a SoulType
	/// </summary>
	public struct SoulData
	{
		public readonly int ProjType;
		public readonly LocalizedText Name; //Needs to include pluralization
		public readonly LocalizedText ToUnlock;
		public readonly LocalizedText Description;

		public readonly float DmgModifier;
		public readonly float KBModifier;
		public readonly Func<bool> Unlocked;

		public string NameSingular => Name.Format(1);
		public string Tooltip => NameSingular
			+ $"\n{AssUISystem.BaseDamageText.Format((int)(EverhallowedLantern.BaseDmg * (DmgModifier + 1f)))}" 
			+ $"\n{AssUISystem.BaseKnockbackText.Format(Math.Round(EverhallowedLantern.BaseKB * (KBModifier + 1f), 1))}"
			+ "\n" + Description.ToString();

		public SoulData(int projType, string internalName, Func<bool> unlocked = null, float dmgModifier = 0f, float kBModifier = 0f)
		{
			ProjType = projType;
			string thisKey = $"SoulData.{internalName}.";
			Name = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}DisplayName"), () => "");
			ToUnlock = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}Unlock"), () => "");
			Description = Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{thisKey}Description"), () => "");
			DmgModifier = dmgModifier;
			KBModifier = kBModifier;
			Unlocked = unlocked ?? (() => true);
		}
	}

	[Content(ContentType.Bosses)]
	public class EverhallowedLantern : MinionItemBase
	{
		public const int BaseDmg = 26;
		public const float BaseKB = 0.5f;

		public static readonly int SetIncrease = SoulSaviorHeaddress.EverhallowedLanternDamageIncrease;

		#region Static Methods
		/// <summary>
		/// Sets the data up for a SoulType
		/// </summary>
		public static SoulData SetSoulData(SoulType selected)
		{
			switch (selected)
			{
				case SoulType.Dungeon:
					return new SoulData
						(
						projType: ModContent.ProjectileType<CompanionDungeonSoulPostWOFMinion>(),
						internalName: "DungeonSoul",
						dmgModifier: 0.1f
						);
				case SoulType.Fright:
					return new SoulData
						(
						projType: ModContent.ProjectileType<CompanionDungeonSoulFrightMinion>(),
						internalName: "SoulofFright",
						unlocked: () => NPC.downedMechBoss3,
						dmgModifier: 0.25f,
						kBModifier: 3f
						);
				case SoulType.Sight:
					return new SoulData
						(
						projType: ModContent.ProjectileType<CompanionDungeonSoulSightMinion>(),
						internalName: "SoulofSight",
						unlocked: () => NPC.downedMechBoss2,
						dmgModifier: -0.15f
						);
				case SoulType.Might:
					return new SoulData
						(
						projType: ModContent.ProjectileType<CompanionDungeonSoulMightMinion>(),
						internalName: "SoulofMight",
						unlocked: () => NPC.downedMechBoss1,
						dmgModifier: 0.55f,
						kBModifier: 7f
						);
				default:
					throw new Exception("No SoulData specified");
			}
		}

		/// <summary>
		/// Holds data about each SoulType
		/// </summary>
		public static SoulData[] DataList;

		/// <summary>
		/// Used to access a particular SoulTypes data
		/// </summary>
		public static SoulData GetSoulData(SoulType selected)
		{
			return DataList[(int)Math.Log((int)selected, 2)];
		}

		public static CircleUIConf GetUIConf()
		{
			List<string> tooltips = new List<string>();
			List<string> toUnlock = new List<string>();
			List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>();
			List<bool> unlocked = new List<bool>();

			int firstValidProjType = -1;
			foreach (SoulType type in Enum.GetValues(typeof(SoulType)))
			{
				if (type != SoulType.None)
				{
					SoulData data = GetSoulData(type);
					firstValidProjType = data.ProjType;
					assets.Add(TextureAssets.Projectile[firstValidProjType]);
					unlocked.Add(data.Unlocked());
					tooltips.Add(data.Tooltip.ToString());
					toUnlock.Add(data.ToUnlock.ToString());
				}
			}

			return new CircleUIConf(firstValidProjType == -1 ? 0 : Main.projFrames[firstValidProjType], -1, assets, unlocked, tooltips, toUnlock);
		}

		/// <summary>
		/// Called in Mod.Load
		/// </summary>
		public static void DoLoad()
		{
			if (!ContentConfig.Instance.Bosses)
			{
				return;
			}

			Array a = Enum.GetValues(typeof(SoulType));
			DataList = new SoulData[a.Length - 1]; //without None
			int i = 0;
			foreach (SoulType type in a)
			{
				if (type != SoulType.None)
				{
					DataList[i++] = SetSoulData(type);
				}
			}
		}

		/// <summary>
		/// Called in Mod.Unload
		/// </summary>
		public static void DoUnload()
		{
			DataList = null;
		}
		#endregion

		public static LocalizedText MechConditionText { get; private set; }
		public static LocalizedText SetBonusBoostText { get; private set; }

		public override void EvenSaferSetStaticDefaults()
		{
			MechConditionText = this.GetLocalization("MechCondition");
			SetBonusBoostText = this.GetLocalization("SetBonusBoost");
		}

		public override void SetDefaults()
		{
			//Defaults for damage, shoot and knockback dont matter too much here, only for the first summon
			//default to PostWOF
			Item.damage = BaseDmg;
			Item.knockBack = BaseKB;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 26;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp; //4 for life crystal
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 0, 95, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPostWOFMinion>();
			Item.shootSpeed = 10f;
			Item.buffType = ModContent.BuffType<CompanionDungeonSoulMinionBuff>();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			SoulType selected = mPlayer.selectedSoulMinionType;
			damage += GetSoulData(selected).DmgModifier;
		}

		public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
		{
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();

			SoulType selected = mPlayer.selectedSoulMinionType;
			knockback *= GetSoulData(selected).KBModifier;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
			SoulType selected = mPlayer.selectedSoulMinionType;
			SoulData soulData = GetSoulData(selected);
			type = soulData.ProjType;

			Vector2 spawnPos = new Vector2(player.Center.X + player.direction * 8f, player.Bottom.Y - 12f);
			Vector2 spawnVelo = new Vector2(player.velocity.X + player.direction * 1.5f, player.velocity.Y - 1f);

			int index = Projectile.NewProjectile(source, spawnPos, spawnVelo, type, damage, knockback, Main.myPlayer, 0f, 0f);
			Main.projectile[index].originalDamage = (int)(Item.damage * (1f + soulData.DmgModifier));
			return false;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation.X = player.Center.X;
			player.itemLocation.Y = player.Bottom.Y + 2f;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//"Summons a Soul to fight for you" is changed for the appropriate type
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
			SoulType selected = mPlayer.selectedSoulMinionType;

			SoulData data = GetSoulData(selected);

			TooltipLine line = new TooltipLine(Mod, "dummy", "dummy");

			for (int i = 0; i < tooltips.Count; i++)
			{
				if (Main.LocalPlayer.HasItem(ModContent.ItemType<EverhallowedLantern>()))
				{
					if (tooltips[i].Mod == "Terraria" && tooltips[i].Name == "ItemName")
					{
						tooltips[i].Text += " (" + data.NameSingular + ")";
					}
				}

				if (tooltips[i].Mod == "Terraria" && tooltips[i].Name == "Tooltip0")
				{
					line = tooltips[i];
				}
			}

			int tooltipIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));

			if (line.Name != "dummy") tooltips.Remove(line);

			bool allUnlocked = true;
			foreach (SoulType type in Enum.GetValues(typeof(SoulType)))
			{
				if (type != SoulType.None)
				{
					data = GetSoulData(type);
					if (!data.Unlocked())
					{
						allUnlocked = false;
					}
				}
			}

			if (!(allUnlocked && Main.LocalPlayer.HasItem(Item.type)))
			{
				tooltips.Insert(tooltipIndex++, new TooltipLine(Mod, nameof(MechConditionText), MechConditionText.ToString()));
			}

			tooltips.Insert(tooltipIndex++, new TooltipLine(Mod, nameof(SetBonusBoostText), SetBonusBoostText.Format(SetIncrease)));
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SoulofNight, 5).AddIngredient(ItemID.SoulofLight, 5).AddIngredient(ModContent.ItemType<EverglowLantern>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
