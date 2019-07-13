using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class PocketSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pocket Sand");
            Tooltip.SetDefault("'Throw a clump of sand at an enemy to confuse it'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ThrowingKnife);
            item.damage = 1;
            item.useTime = 35;
            item.shootSpeed = 4.5f;
            item.shoot = mod.ProjectileType("PocketSand");
            item.useAnimation = 35;
            item.autoReuse = true;
            item.value = 0;
            item.rare = -11;
            item.noUseGraphic = true;
            item.value = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SandBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 10); //makes 10 instead of one per crafting operation
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack)) //spawns an accurate projectile once
            {
                for (int i = 0; i < 2; i++) //spawn two more with random velocity if first one is actually spawned
                {
                    //Vector2 cm = new Vector2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
                    Vector2 cm = new Vector2(speedX, speedY);
                    float randx = Main.rand.NextFloat(0.8f, 1.2f);
                    float randx2 = Main.rand.NextFloat(-1.1f, 1.1f);
                    float randy = Main.rand.NextFloat(0.8f, 1.2f);
                    float bobandy = Main.rand.NextFloat(-1.1f, 1.1f);
                    float velox = ((cm.X * item.shootSpeed * randx) / cm.Length()) + randx2; //first rand makes it so it has different velocity factor (how far it flies)
                    float veloy = ((cm.Y * item.shootSpeed * randy) / cm.Length()) + bobandy; //second rand is a kinda offset used mainly for when shooting vertically or horizontally
                    Vector2 velo = new Vector2(velox, veloy);
                    Projectile.NewProjectile(position, velo, type, damage, knockBack);
                }
                return true;
            }
            return false;
        }
    }
}
