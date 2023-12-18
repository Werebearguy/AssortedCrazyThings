using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[LegacyName("SigilOfRetreat")]
	public class SigilOfTheTalon : SigilItemBase
	{
		public static readonly int FirstDamageDropOff = 75;

		//If this is changed make sure to doublecheck SigilOfTheTalonGlobalProj netcode
		public static readonly int MaxPierce = 3;

		public static LocalizedText HookDamageText { get; private set; }

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxPierce);

		public override void EvenSaferSetStaticDefaults()
		{
			HookDamageText = this.GetLocalization("HookDamage");
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SigilOfTheWing>();
		}

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.width = 22;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AssPlayer>().sigilOfTheTalon = true;
		}

		public static void ModifyTooltip(Mod mod, List<TooltipLine> tooltips, string prefix = "")
		{
			int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
			if (insertIndex == -1) insertIndex = tooltips.Count - 1;
			insertIndex++;

			Player player = Main.LocalPlayer;
			var mPlayer = player.GetModPlayer<AssPlayer>();
			int damage = Math.Max(1, (int)(mPlayer.LastSelectedWeaponDamage * FirstDamageDropOff / 100f));

			string text = prefix + HookDamageText.Format(FirstDamageDropOff,  damage);
			tooltips.Insert(insertIndex, new TooltipLine(mod, nameof(HookDamageText), text));
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			ModifyTooltip(Mod, tooltips);
		}
	}
}
