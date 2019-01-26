using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class YoungWyvern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyverntail");
            Tooltip.SetDefault("Summons a friendly Young Wyvern that flies with you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("YoungWyvern");
            item.buffType = mod.BuffType("YoungWyvern");
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wyverntail, 1);
            recipe.AddTile(TileID.WorkBenches);
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
