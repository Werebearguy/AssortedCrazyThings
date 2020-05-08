using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class TrueLegendaryWoodenSwordProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Legendary Wooden Sword");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            projectile.aiStyle = 2; //6 for powder, 2 for throwing knife
            projectile.height = 20;
            projectile.width = 20;
            projectile.alpha = 255;
            //projectile.penetrate = 2;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;

            //drawOriginOffsetX = 0;
            //drawOffsetX = (int)0;
            drawOriginOffsetX = -(projectile.width / 2 - 50f / 2);
            drawOffsetX = (int)-drawOriginOffsetX * 2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position - Vector2.Normalize(projectile.velocity) * 30f, 50, 50, 169, projectile.velocity.X, projectile.velocity.Y, 100, Color.White, 1.25f);
                dust.noGravity = true;
            }
            Main.PlaySound(SoundID.Dig, projectile.position);
            return true;
        }

        public override void PostAI()
        {
            if (projectile.ai[0] < 15) projectile.ai[0] = 15;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
            else
            {
                //162 for "sparks"
                //169 for just light
                int dustType = 169;
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.Hitbox.X, projectile.Hitbox.Y) - Vector2.Normalize(projectile.velocity) * 40f, projectile.Hitbox.Width, projectile.Hitbox.Height, dustType, projectile.velocity.X, projectile.velocity.Y, 100, Color.White, 1.25f);
                dust.noGravity = true;
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 0.785f;
            //projectile.rotation = 0;
        }
    }
}
