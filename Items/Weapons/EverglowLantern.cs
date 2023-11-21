using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Bosses)]
	public class EverglowLantern : MinionItemBase
	{
		public override void SetDefaults()
		{
			//Defaults for damage, shoot and knockback dont matter too much here
			//default to PreWol
			Item.damage = EverhallowedLantern.BaseDmg / 2 - 1;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 22;
			Item.height = 38;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.HoldUp; //4 for life crystal
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = 3;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPreWOFMinion>();
			Item.shootSpeed = 10f;
			Item.knockBack = EverhallowedLantern.BaseKB;
			Item.buffType = ModContent.BuffType<CompanionDungeonSoulMinionBuff>();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				return false;
			}

			//one that shoots out far 
			int index = Projectile.NewProjectile(source, player.Center.X + player.direction * 8f, player.Bottom.Y - 12f, player.velocity.X + player.direction * 1.5f, player.velocity.Y - 1f, type, damage, knockback, Main.myPlayer, 0f, 0f);
			Main.projectile[index].originalDamage = Item.damage;

			//one that shoots out less
			index = Projectile.NewProjectile(source, player.Center.X + player.direction * 8f, player.Bottom.Y - 10f, player.velocity.X + player.direction * 1, player.velocity.Y - 1 / 2f, type, damage, knockback, Main.myPlayer, 0f, 0f);
			Main.projectile[index].originalDamage = Item.damage;

			return false;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation.X = player.Center.X;
			player.itemLocation.Y = player.Bottom.Y + 2f;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//need a dummy because you can't remove elements from a list while you are iterating
			string[] newDamage;
			string tempString;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "Damage")
				{
					try //try catch in case some other mods modify it
					{
						//split string up into words
						newDamage = line2.Text.Split(new string[] { " " }, 10, StringSplitOptions.RemoveEmptyEntries);

						//rebuild text string and add "x 2" after the damage number
						tempString = newDamage[0] + " x 2";

						//add remaining words back
						for (int i = 1; i < newDamage.Length; i++)
						{
							tempString += " " + newDamage[i];
						}
						line2.Text = tempString;
					}
					catch (Exception)
					{

					}
				}
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 25).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 10).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddTile(TileID.Anvils).Register();
		}
	}
}
