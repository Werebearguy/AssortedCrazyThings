using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetSunItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Sun");
            Tooltip.SetDefault("Summons a small sun that provides you with constant sunlight"
                +"\nShows the current time in the buff tip");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("PetSunProj");
            item.buffType = mod.BuffType("PetSunBuff");
            item.width = 20;
            item.height = 26;
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddIngredient(ItemID.SunStone);
            recipe.AddIngredient(ItemID.Hellstone, 25);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
