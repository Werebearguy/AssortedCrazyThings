using System.Collections.Generic;
using AssortedCrazyThings.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
	public class SlimeBeaconItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Beacon");
            Tooltip.SetDefault("'Do The Slime With Me!'"); //This is what the party machine says but with "Balloons" instead of "Slimes"
        }

        public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = -11;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.createTile = mod.TileType<SlimeBeaconTile>();
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //new Color(255, 100, 30, 255)
            if (Main.netMode == NetmodeID.MultiplayerClient) tooltips.Add(new TooltipLine(mod, "Multi", "[c/FFA01D:DOES NOT WORK IN MULTIPLAYER]"));
        }

        public override void AddRecipes()
		{
            //Maybe sold by party girl?

			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.LunarBar, 12);
			//recipe.AddTile(TileID.LunarCraftingStation);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		}
	}
}