using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class TrueLegendaryWoodenSword : ModItem
	{
        public static int ProjDamage = 15;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Legendary Wooden Sword");
            Tooltip.SetDefault("'Truly Legendary'");
        }

		public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.CobaltSword);
            item.width = 50;
            item.height = 50;
            item.rare = -11;
            item.value = Item.sellPrice(0, 2, 25, 0); //2 gold for broken, 25 silver for legendary
            item.shoot = mod.ProjectileType<TrueLegendaryWoodenSwordProj>();
            item.shootSpeed = 10f; //fairly short range, similar to throwing knife
		}

        //public override void HoldItem(Player player)
        //{
        //    player.itemLocation.X += -player.direction * 3; 
        //}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //162 for "sparks"
            //169 for just light
            int dustType = 169;
            int dustid = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType, player.velocity.X * 0.2f + (player.direction * 3), player.velocity.Y * 0.2f, 100, Color.White, 1.25f);
            Main.dust[dustid].noGravity = true;
            Main.dust[dustid].velocity *= 2f;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<LegendaryWoodenSword>(), 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(player.Center + Utils.SafeNormalize(new Vector2(speedX, speedY), default(Vector2)) * 30f, new Vector2(speedX, speedY), item.shoot, ProjDamage, item.knockBack, Main.myPlayer);
            return false;
        }
    }
}
