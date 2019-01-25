using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class AbeeminationItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination +");
            Tooltip.SetDefault("Summons a friendly Abeemination to follow you.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("Abeemination");
            item.buffType = mod.BuffType("AbeeminationBuff");
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Abeemination, 1);
            recipe.AddIngredient(ItemID.LifeFruit, 1);
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
