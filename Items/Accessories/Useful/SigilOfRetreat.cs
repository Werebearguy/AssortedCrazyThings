using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class SigilOfRetreat : SigilItemBase
	{
		public override void EvenSaferSetStaticDefaults()
		{
			DisplayName.SetDefault("Sigil of Retreat");
			Tooltip.SetDefault("Teleports you home when health is dangerously low"
				+ "\nHas a cooldown of " + (AssPlayer.TeleportHomeTimerMax / 60) + " minutes");
		}

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();

			bool inVanitySlot = false;

			for (int i = 0; i < tooltips.Count; i++)
			{
				if (tooltips[i].Name == "SocialDesc")
				{
					inVanitySlot = true;
					tooltips[i].Text = "Cooldown will go down while in social slot";
					break;
				}
			}

			int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
			if (insertIndex == -1) insertIndex = tooltips.Count;

			if (!inVanitySlot)
			{
				for (int i = 0; i < tooltips.Count; i++)
				{
					if (tooltips[i].Name == "Tooltip1")
					{
						insertIndex = i + 1; //it inserts "left" of where it found the index (without +1), so everything else get pushed one up
						break;
					}
				}
			}

			if (Main.LocalPlayer.ItemInInventoryOrEquipped(Item))
			{
				if (mPlayer.canTeleportHome)
				{
					tooltips.Insert(insertIndex, new TooltipLine(Mod, "Ready", "Ready to use"));
				}
				else
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
						tooltips.Insert(insertIndex, new TooltipLine(Mod, "Ready", "Ready again in " + Math.Round(mPlayer.teleportHomeTimer / 60f) + timeName + dots));
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
						tooltips.Insert(insertIndex, new TooltipLine(Mod, "Ready", "Ready again in " + mPlayer.teleportHomeTimer + timeName + dots));
					}
				}
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().teleportHome = true;
		}
	}
}
