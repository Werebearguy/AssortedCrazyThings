using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    [Autoload]
    public class TomeOfShadowflameSkulls : AssItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BookofSkulls);
            Item.damage = 50;
            Item.mana = 6;
            Item.useTime = 35;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.ClothiersCurse;
            Item.useAnimation = 35;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = -11;
            Item.noUseGraphic = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Shadowflame Skulls");
            Tooltip.SetDefault("Inflicts Shadowflame on enemies");
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SoulofFright, 10).AddIngredient(ItemID.BookofSkulls, 1).AddTile(TileID.CrystalBall).Register();
        }
    }
}
