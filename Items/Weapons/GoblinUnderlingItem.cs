using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingItem : AssItem
	{
		public const int BaseDmg = 8;
		public const float BaseKB = 1.5f;
		public static readonly Color apricotColor = new Color(251, 206, 177);

		public GoblinUnderlingClass currentClass;

		//TODO goblin common tooltip:
		/*
		 * Class can be changed with [c/{0}:<right>]
			Gets stronger throughout progression
			Only one can be summoned
		*/
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(apricotColor.Hex3());

		public abstract int ProjType { get; }

		public abstract int BuffType { get; }

		public static HashSet<int> Items { get; private set; }
		public static Dictionary<int, int> BuffToItem { get; private set; }
		public static Dictionary<int, int> BuffToProjectile { get; private set; }

		public override void Load()
		{
			Items ??= new();
			BuffToItem ??= new();
			BuffToProjectile ??= new();
		}

		public override void Unload()
		{
			Items = null;
			BuffToItem = null;
			BuffToProjectile = null;
		}

		public sealed override void SetStaticDefaults()
		{
			Items.Add(Item.type);
			BuffToItem.Add(Item.buffType, Item.type);
			BuffToProjectile.Add(Item.buffType, Item.shoot);

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			Item.damage = BaseDmg;
			Item.knockBack = BaseKB;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ProjType;
			Item.shootSpeed = 0f;
			Item.buffType = BuffType;

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{
			//Set currentClass here, and size/rare/value
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
			{
				mult = 0;
			}
		}

		public override void UseAnimation(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.UseSound = SoundID.MaxMana.WithVolumeScale(0.8f);
			}
			else
			{
				Item.UseSound = SoundID.Item44;
			}
		}

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				return 3f;
			}
			return base.UseSpeedMultiplier(player);
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((byte)currentClass);
		}

		public override void NetReceive(BinaryReader reader)
		{
			currentClass = (GoblinUnderlingClass)reader.ReadByte();
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("currentClass", (byte)currentClass);
		}

		public override void LoadData(TagCompound tag)
		{
			byte val = tag.GetByte("currentClass");
			if (!Enum.IsDefined(typeof(GoblinUnderlingClass), val))
			{
				val = 0;
			}
			currentClass = (GoblinUnderlingClass)val;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			//This is purely done for the tooltip
			damage *= GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass).damageMult;
		}

		public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
		{
			//This is purely done for the tooltip
			knockback *= GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass).knockbackMult;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				byte val = (byte)currentClass;
				val++;
				if (!Enum.IsDefined(typeof(GoblinUnderlingClass), val))
				{
					val = 0;
				}
				currentClass = (GoblinUnderlingClass)val;
				Main.NewText(AssLocalization.SelectedText.Format(AssLocalization.GetEnumText(currentClass)), apricotColor);

				if (player.ownedProjectileCounts[type] > 0)
				{
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile other = Main.projectile[i];
						if (other.active && other.owner == player.whoAmI && other.type == type && other.ModProjectile is GoblinUnderlingProj underling)
						{
							underling.currentClass = currentClass;
							other.NetSync();
						}
					}
				}

				return false;
			}

			player.AddBuff(Item.buffType, 2);

			if (player.ownedProjectileCounts[type] > 0)
			{
				//Use always resummons
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile other = Main.projectile[i];
					if (other.active && other.owner == player.whoAmI && other.type == type)
					{
						other.Kill();
					}
				}
			}

			knockback = Item.knockBack; //Use baseline values
			int origDamage = Item.damage;

			int index = Projectile.NewProjectile(source, position, Vector2.UnitX * player.direction, type, damage, knockback, Main.myPlayer);
			Main.projectile[index].originalDamage = origDamage;
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int nameIndex = tooltips.FindIndex(t => t.Mod == "Terraria" && t.Name == "ItemName");
			if (nameIndex > -1)
			{
				tooltips[nameIndex].Text = AssLocalization.ConcatenateTwoText.Format(tooltips[nameIndex].Text, $"({AssLocalization.GetEnumText(currentClass)})");
			}
		}
	}
}
