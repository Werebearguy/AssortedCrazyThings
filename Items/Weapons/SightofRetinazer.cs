using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class SightofRetinazer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sight of Retinazer");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LaserRifle);
            item.width = 56;
            item.height = 26;
            item.damage = 40;
            item.mana = 0;
            item.shoot = ProjectileID.MiniRetinaLaser;
            item.shootSpeed = 15f;
            item.noMelee = true;
            item.ranged = true;
            item.useAnimation = 10;
            item.useTime = 10;
            item.value = Item.sellPrice(gold: 1);
            item.rare = -11;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.LaserRifle, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }
    }
}
