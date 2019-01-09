using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileFracturedEyeRed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unconscious Fractured Eye");
            Tooltip.SetDefault("Summons a docile red Fractured Eye to follow you.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("DocileFracturedEyeRed");
            item.buffType = mod.BuffType("DocileFracturedEyeRed");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BlackLens, 1);
            recipe.AddIngredient(ItemID.Lens, 2);
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
