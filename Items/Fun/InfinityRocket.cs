using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
    public class InfinityRocket : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinity Rocket");
            Tooltip.SetDefault("'It seriously never ends!'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.EndlessMusketPouch);
            item.ammo = AmmoID.Rocket;
            item.rare = -11;
            item.shoot = ProjectileID.None;
            item.damage = 40;
            item.UseSound = SoundID.Item11;
            //item.shoot = ProjectileID.RocketII;
            item.value = Item.sellPrice(gold: 4);

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RocketI, 3996);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
