using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class SigilOfLastStand : SigilItemBase
	{
		public override void EvenSaferSetStaticDefaults()
		{
			DisplayName.SetDefault("Sigil of Last Stand");
			Tooltip.SetDefault("Combines the effects of Sigil of Retreat and Sigil of Pain Suppression"
				+ "\nHas a cooldown of " + (AssPlayer.TeleportHomeTimerMax / 60) + " minutes");
		}

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 2, 0, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//tooltip based off of the teleport ability
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();

			for (int i = 0; i < tooltips.Count; i++)
			{
				if (tooltips[i].Name == "SocialDesc")
				{
					tooltips[i].Text = "Cooldown will go down while in social slot";
					break;
				}
			}

			int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
			if (insertIndex == -1) insertIndex = tooltips.Count - 1;
			insertIndex++;

			if (Main.LocalPlayer.ItemInInventoryOrEquipped(Item))
			{
				if (mPlayer.canTeleportHome && mPlayer.SigilOfTheWingReady)
				{
					tooltips.Insert(insertIndex, new TooltipLine(Mod, "Ready", "Ready to use"));
				}

				if (!mPlayer.SigilOfTheWingReady)
				{
					//create animating "..." effect after the Ready line
					string dots = "";
					int dotCount = ((int)Main.GameUpdateCount % 120) / 30; //from 0 to 30, from 31 to 60, from 61 to 90

					for (int i = 0; i < dotCount; i++)
					{
						dots += ".";
					}

					string timeName;
					if (mPlayer.sigilOfTheWingCooldown > 60 * 60) //more than 1 minute
					{
						if (mPlayer.sigilOfTheWingCooldown > 90 * 60) //more than 1:30 minutes because of round
						{
							timeName = " minutes";
						}
						else
						{
							timeName = " minute";
						}
						tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Ready2", "Pain supression: Ready again in " + Math.Round(mPlayer.sigilOfTheWingCooldown / (60f * 60f)) + timeName + dots));
					}
					else
					{
						if (mPlayer.sigilOfTheWingCooldown > 60) //more than 1 second
						{
							timeName = " seconds";
						}
						else
						{
							timeName = " second";
						}
						tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Ready2", "Pain supression: Ready again in " + Math.Round(mPlayer.sigilOfTheWingCooldown / 60f) + timeName + dots));
					}
				}

				if (!mPlayer.canTeleportHome)
				{
					//create animating "..." effect after the Ready line
					string dots = "";
					int dotCount = ((int)Main.GameUpdateCount % 120) / 30; //from 0 to 30, from 31 to 60, from 61 to 90

					for (int i = 0; i < dotCount; i++)
					{
						dots += ".";
					}

					string timeName;
					if (mPlayer.teleportHomeTimer > 60) //more than 1 minute
					{
						if (mPlayer.teleportHomeTimer > 90) //more than 1:30 minutes because of round
						{
							timeName = " minutes";
						}
						else
						{
							timeName = " minute";
						}
						tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Ready1", "Retreat: Ready again in " + Math.Round(mPlayer.teleportHomeTimer / 60f) + timeName + dots));
					}
					else
					{
						if (mPlayer.teleportHomeTimer > 1) //more than 1 second
						{
							timeName = " seconds";
						}
						else
						{
							timeName = " second";
						}
						tooltips.Insert(insertIndex++, new TooltipLine(Mod, "Ready1", "Retreat: Ready again in " + mPlayer.teleportHomeTimer + timeName + dots));
					}
				}
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().sigilOfTheWing = true;
			player.GetModPlayer<AssPlayer>().teleportHome = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SigilOfRetreat>()).AddIngredient(ModContent.ItemType<SigilOfTheWing>()).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
