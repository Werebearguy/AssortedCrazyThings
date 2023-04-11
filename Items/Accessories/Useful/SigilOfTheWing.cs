using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[LegacyName("SigilOfPainSuppression")]
	public class SigilOfTheWing : SigilItemBase
	{
		public static readonly int DurationSeconds = 5;
		public static readonly int HealthRestoreAmount = 25;
		public static readonly int CooldownSeconds = 6 * 60;

		//TODO fix tooltip
		/*($"On death, transform into a soul for {DurationSeconds} seconds, regenerating {HealthRestoreAmount}% max health"
				+ "\nWhile transformed, you cannot use items"
				+ $"\nHas a cooldown of {CooldownSeconds / 60} minutes"); */

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.width = 30;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}

		public static void ModifyTooltip(Mod mod, Item item, List<TooltipLine> tooltips, string prefix = "")
		{
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

			if (Main.LocalPlayer.ItemInInventoryOrEquipped(item))
			{
				if (mPlayer.SigilOfTheWingReady)
				{
					tooltips.Insert(insertIndex, new TooltipLine(mod, "Ready", prefix + "Effect ready"));
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
						tooltips.Insert(insertIndex++, new TooltipLine(mod, "Ready2", prefix + "Ready again in " + Math.Round(mPlayer.sigilOfTheWingCooldown / (60f * 60f)) + timeName + dots));
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
						tooltips.Insert(insertIndex++, new TooltipLine(mod, "Ready2", prefix + "Ready again in " + Math.Round(mPlayer.sigilOfTheWingCooldown / 60f) + timeName + dots));
					}
				}
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			ModifyTooltip(Mod, Item, tooltips);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().sigilOfTheWing = true;
		}
	}
}
