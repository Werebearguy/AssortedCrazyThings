using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[LegacyName("SigilOfLastStand")]
	public class SigilOfTheHarvester : SigilItemBase
	{
		//TODO fix tooltip
		/*("'The aspects of the Harvester reside within you'"
				+ "\nSummons fractured souls to seek out enemies in combat"
				+ "\nFrequency and intensity increases the lower your health gets"
				+ $"\nOn death, transform into a soul for {SigilOfTheWing.DurationSeconds} seconds, regenerating {SigilOfTheWing.HealthRestoreAmount}% max health"
				+ "\nWhile transformed, you cannot use items"
				+ $"\nHas a cooldown of {SigilOfTheWing.CooldownSeconds / 60} minutes"
				+ "\nAllows your grappling hooks to deal damage when extending"
				+ "\nCan hit up to 3 enemies"); */

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
			SigilOfTheTalon.ModifyTooltip(Mod, tooltips, "Sigil of the Talon: ");
			SigilOfTheWing.ModifyTooltip(Mod, Item, tooltips, "Sigil of the Wing: ");
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
