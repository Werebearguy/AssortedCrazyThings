using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class TomeOfShadowflameSkulls : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.BookofSkulls);
            item.damage = 50;
            item.mana = 6;
            item.useTime = 35;
            item.shootSpeed = 10f;
            item.shoot = ProjectileID.ClothiersCurse;
            item.useAnimation = 35;
            item.value = Item.sellPrice(silver: 10);
            item.rare = -11;
            item.noUseGraphic = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Shadowflame Skulls");
            Tooltip.SetDefault("Inflicts Shadowflame on enemies");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddIngredient(ItemID.BookofSkulls, 1);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
