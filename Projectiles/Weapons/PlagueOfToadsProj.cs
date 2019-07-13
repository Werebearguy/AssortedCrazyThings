using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PlagueOfToadsProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads Fired");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.ignoreWater = true;
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 1; //-1
            projectile.timeLeft = 300;
            projectile.scale = 1f;
            projectile.magic = true;
            projectile.extraUpdates = 1;
            projectile.alpha = 50;
        }

        public override void Kill(int timeLeft)
        {
            Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.Center.X, projectile.Bottom.Y - 2f), 2, 2, 154, 0f, 0f, 38, Color.LightGreen, 1f)];
            dust.velocity = new Vector2(Main.rand.NextFloat(2) - 1f, Main.rand.NextFloat(2) - 1f);
            dust.scale = 0.95f;
        }

        public override void AI()
        {
            projectile.spriteDirection = ((int)projectile.ai[0] % 2 == 0) ? 1 : -1;
            projectile.rotation -= projectile.spriteDirection * 2.5f * projectile.ai[1];
            projectile.velocity.Y *= 1.005f + (projectile.ai[0] / 1000);
        }
    }
}
