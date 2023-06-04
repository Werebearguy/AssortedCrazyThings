using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[LegacyName("SigilOfLastStand")]
	public class SigilOfTheHarvester : SigilItemBase
	{
		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(SigilOfTheWing.DurationSeconds, SigilOfTheWing.HealthRestoreAmount, SigilOfTheWing.CooldownSeconds, SigilOfTheTalon.MaxPierce);

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = 5;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string divider = AssUISystem.GetColon();
			SigilOfTheTalon.ModifyTooltip(Mod, tooltips, ModContent.GetInstance<SigilOfTheTalon>().DisplayName.ToString() + divider);
			SigilOfTheWing.ModifyTooltip(Mod, Item, tooltips, ModContent.GetInstance<SigilOfTheWing>().DisplayName.ToString() + divider);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			AssPlayer assPlayer = player.GetModPlayer<AssPlayer>();
			assPlayer.sigilOfTheBeak = Item;
			assPlayer.sigilOfTheBeakDamage = SigilOfTheBeak.HMDamage;
			assPlayer.sigilOfTheWing = true;
			assPlayer.sigilOfTheTalon = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SigilOfTheBeak>()).AddIngredient(ModContent.ItemType<SigilOfTheWing>()).AddIngredient(ModContent.ItemType<SigilOfTheTalon>()).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
