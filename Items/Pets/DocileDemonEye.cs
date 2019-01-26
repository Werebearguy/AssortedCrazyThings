using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileDemonEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Tooltip.SetDefault("Summons a docile Demon Eye to follow you"
                + "\nChange its appearance with a Demon Eye Contact Case");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.width = 34;
            item.height = 22;
            item.shoot = mod.ProjectileType("DocileDemonEyeProj");
            item.buffType = mod.BuffType("DocileDemonEyeBuff");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BlackLens, 1);
            recipe.AddIngredient(ItemID.Lens, 1);
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
