using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[LegacyName("SigilOfPainSuppression")]
	public class SigilOfTheWing : SigilItemBase
	{
		public static readonly int DurationSeconds = 5;
		public static readonly int HealthRestoreAmount = 25;
		public static readonly int CooldownSeconds = 6 * 60;

		public static LocalizedText SocialDescText { get; private set; }
		public static LocalizedText EffectReadyText { get; private set; }
		public static LocalizedText ReadyAgainInMinutesText { get; private set; }
		public static LocalizedText ReadyAgainInSecondsText { get; private set; }

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DurationSeconds, HealthRestoreAmount, CooldownSeconds);

		public override void EvenSaferSetStaticDefaults()
		{
			//TODO loca
			SocialDescText = this.GetLocalization("SocialDesc");
			EffectReadyText = this.GetLocalization("EffectReady");
			ReadyAgainInMinutesText = this.GetLocalization("ReadyAgainInMinutes");
			ReadyAgainInSecondsText = this.GetLocalization("ReadyAgainInSeconds");
		}

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
					tooltips[i].Text = SocialDescText.ToString();
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
					tooltips.Insert(insertIndex, new TooltipLine(mod, nameof(EffectReadyText), prefix + EffectReadyText.ToString()));
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

					if (mPlayer.sigilOfTheWingCooldown > 60 * 60) //more than 1 minute
					{
						tooltips.Insert(insertIndex++, new TooltipLine(mod, nameof(ReadyAgainInMinutesText), prefix + ReadyAgainInMinutesText.Format((int)Math.Round(mPlayer.sigilOfTheWingCooldown / (60f * 60f))) + dots));
					}
					else
					{
						tooltips.Insert(insertIndex++, new TooltipLine(mod, nameof(ReadyAgainInSecondsText), prefix + ReadyAgainInSecondsText.Format(Math.Round(mPlayer.sigilOfTheWingCooldown / 60f)) + dots));
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
