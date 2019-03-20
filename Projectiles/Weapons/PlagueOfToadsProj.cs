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
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.scale = 1f;
            projectile.magic = true;
            projectile.extraUpdates = 1;
            projectile.alpha = 50;
        }

        public override void Kill(int timeLeft)
        {
            Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + projectile.height - 2f), 2, 2, 154, 0f, 0f, 38, default(Color), 1f)];
            dust.position.X -= 2f;
            dust.velocity += -projectile.oldVelocity * 0.25f;
            dust.scale = 0.95f;
        }

        public override void AI()
        {
            projectile.spriteDirection = projectile.ai[0] == 1f ? 1: -1;
            projectile.rotation += -projectile.spriteDirection * projectile.velocity.Y * projectile.ai[1];
        }
    }
}
