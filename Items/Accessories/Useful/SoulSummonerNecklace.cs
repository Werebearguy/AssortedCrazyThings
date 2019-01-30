using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class SoulSummonerNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Summoner's Necklace");
			Tooltip.SetDefault("15% increased summon damage"
				+ "\nIncreases your max number of minions by 3");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.value = Item.sellPrice(gold: 5);
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.maxMinions++;
            player.maxMinions++;
			player.minionDamage += 0.15f;
        }

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<SoulEmblem>(), 1);
            recipe.AddIngredient(mod.ItemType<SoulNecklace>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
