using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class ChunkyandMeatball : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky and Meatball");
            Tooltip.SetDefault("Summons a pair of inseperable brothers to follow you.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("ChunkyProj");
            item.buffType = mod.BuffType("ChunkyandMeatball");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 4);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "ChunkysEye");
            recipe.AddIngredient(mod, "MeatballsEye");
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
