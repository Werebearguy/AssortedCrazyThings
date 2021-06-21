using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
    [Autoload]
    public class InfinityRocket : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinity Rocket");
            Tooltip.SetDefault("'It seriously never ends!'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.EndlessMusketPouch);
            Item.ammo = AmmoID.Rocket;
            Item.rare = -11;
            Item.shoot = ProjectileID.None;
            Item.damage = 40;
            Item.UseSound = SoundID.Item11;
            //item.shoot = ProjectileID.RocketII;
            Item.value = Item.sellPrice(gold: 4);

        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.RocketI, 3996).AddTile(TileID.CrystalBall).Register();
        }
    }
}
