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
                int dustid = Dust.NewDust(projectile.position - Vector2.Normalize(projectile.velocity) * 30f, 50, 50, 169, projectile.velocity.X, projectile.velocity.Y, 100, Color.White, 1.25f);
                Main.dust[dustid].noGravity = true;
            }
            Main.PlaySound(0, projectile.position);
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //if (Main.rand.NextFloat() >= .50f)
            //{
            //    target.AddBuff(BuffID.Confused, 90); //1 1/2 seconds, 50% chance
            //}
        }

        //private void SpawnSandDust(Color color, Rectangle hitbox, Player player, float velox)
        //{
        //    //the thick no-outline one that spreads
        //    int dustid = Dust.NewDust(new Vector2((float)hitbox.X, (float)(hitbox.Y - player.height / 2)), hitbox.Width, hitbox.Height, 102, player.velocity.X * 0.2f + (Main.rand.NextBool() ? velox : -velox), player.velocity.Y * 0.2f, 150, color, 1f);
        //    Main.dust[dustid].noGravity = false;
        //    Main.dust[dustid].fadeIn = 0.8f;
        //    //the outline one 
        //    int dustid2 = Dust.NewDust(new Vector2((float)hitbox.X, (float)(hitbox.Y - player.height / 2)), hitbox.Width, hitbox.Height, 32, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 200, color, 0.8f);
        //    Main.dust[dustid2].noGravity = true;
        //}

        //public override void ModifyDamageHitbox(ref Rectangle hitbox)
        //{
        //    //increase hitbox used in colliding with NPCs while projectile life depletes
        //    hitbox.Width += (int)((LifeTime - projectile.timeLeft) * 1.5f);
        //    hitbox.Height += (int)((LifeTime - projectile.timeLeft) * 1.5f);
        //    hitbox.X -= (int)((LifeTime - projectile.timeLeft) * 1.5f / 2f);
        //    hitbox.Y -= (int)((LifeTime - projectile.timeLeft) * 1.5f / 2f);
        //}

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
                int dustid = Dust.NewDust(new Vector2(projectile.Hitbox.X, projectile.Hitbox.Y) - Vector2.Normalize(projectile.velocity) * 40f, projectile.Hitbox.Width, projectile.Hitbox.Height, dustType, projectile.velocity.X, projectile.velocity.Y, 100, Color.White, 1.25f);
                Main.dust[dustid].noGravity = true;
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 0.785f;
            //projectile.rotation = 0;
        }
    }
}
