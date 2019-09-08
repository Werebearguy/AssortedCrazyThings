using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PocketSand : ModProjectile
    {
        private static readonly int LifeTime = 30;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/BlankProjectile";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pocket Sand");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            projectile.aiStyle = 2; //6 for powder, 2 for throwing knife
            projectile.height = 20;
            projectile.width = 20;
            projectile.timeLeft = LifeTime;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.NextFloat() >= .50f)
            {
                target.AddBuff(BuffID.Confused, 120); //2 seconds, 50% chance
            }
        }

        private void SpawnSandDust(Color color, Rectangle hitbox, Player player, float velox)
        {
            //the thick no-outline one that spreads
            Dust dust = Dust.NewDustDirect(new Vector2((float)hitbox.X, (float)(hitbox.Y - player.height / 2)), hitbox.Width, hitbox.Height, 102, player.velocity.X * 0.2f + (Main.rand.NextBool() ? velox : -velox), player.velocity.Y * 0.2f, 150, color, 1f);
            dust.noGravity = false;
            dust.fadeIn = 0.8f;
            //the outline one 
            dust = Dust.NewDustDirect(new Vector2((float)hitbox.X, (float)(hitbox.Y - player.height / 2)), hitbox.Width, hitbox.Height, 32, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 200, color, 0.8f);
            dust.noGravity = true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            //increase hitbox used in colliding with NPCs while projectile life depletes
            hitbox.Width += (int)((LifeTime - projectile.timeLeft) * 1.5f);
            hitbox.Height += (int)((LifeTime - projectile.timeLeft) * 1.5f);
            hitbox.X -= (int)((LifeTime - projectile.timeLeft) * 1.5f / 2f);
            hitbox.Y -= (int)((LifeTime - projectile.timeLeft) * 1.5f / 2f);
        }

        public override void PostAI()
        {
            //dont spawn the dust instantly when projectile spawns, give it 1/12th of a second
            if (projectile.timeLeft < LifeTime - 5)
            {
                SpawnSandDust(Color.White, projectile.Hitbox, projectile.GetOwner(), (LifeTime - projectile.timeLeft) * 0.1f);
            }
        }
    }
}
