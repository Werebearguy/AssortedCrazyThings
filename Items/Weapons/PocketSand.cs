using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    [Content(ContentType.Weapons)]
    public class PocketSand : AssItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pocket Sand");
            Tooltip.SetDefault("'Throw a clump of sand at an enemy to confuse it'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ThrowingKnife);
            Item.damage = 1;
            Item.useTime = 35;
            Item.shootSpeed = 4.5f;
            Item.shoot = ModContent.ProjectileType<PocketSandProj>();
            Item.useAnimation = 35;
            Item.autoReuse = true;
            Item.rare = -11;
            Item.noUseGraphic = true;
            Item.value = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10).AddIngredient(ItemID.SandBlock, 1).AddTile(TileID.WorkBenches).Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++) //spawn two more with random velocity if first one is actually spawned
            {
                //Vector2 cm = new Vector2(Main.MouseWorld.X - player.Center.X, Main.MouseWorld.Y - player.Center.Y);
                Vector2 cm = velocity;
                float randx = Main.rand.NextFloat(0.8f, 1.2f);
                float randx2 = Main.rand.NextFloat(-1.1f, 1.1f);
                float randy = Main.rand.NextFloat(0.8f, 1.2f);
                float bobandy = Main.rand.NextFloat(-1.1f, 1.1f);
                float velox = ((cm.X * Item.shootSpeed * randx) / cm.Length()) + randx2; //first rand makes it so it has different velocity factor (how far it flies)
                float veloy = ((cm.Y * Item.shootSpeed * randy) / cm.Length()) + bobandy; //second rand is a kinda offset used mainly for when shooting vertically or horizontally
                Vector2 velo = new Vector2(velox, veloy);
                Projectile.NewProjectile(source, position, velo, type, damage, knockback, Main.myPlayer); //TODO mention bugfix no owner in MP
            }
            return true;
        }
    }
}
