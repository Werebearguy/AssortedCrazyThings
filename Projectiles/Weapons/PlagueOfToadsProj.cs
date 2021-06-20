using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
    public class PlagueOfToadsProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plague of Toads Fired");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1; //-1
            Projectile.timeLeft = 300;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 50;
        }

        public override void Kill(int timeLeft)
        {
            Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Bottom.Y - 2f), 2, 2, 154, 0f, 0f, 38, Color.LightGreen, 1f);
            dust.velocity = new Vector2(Main.rand.NextFloat(2) - 1f, Main.rand.NextFloat(2) - 1f);
            dust.scale = 0.95f;
        }

        public override void AI()
        {
            Projectile.spriteDirection = ((int)Projectile.ai[0] % 2 == 0) ? 1 : -1;
            Projectile.rotation -= Projectile.spriteDirection * 2.5f * Projectile.ai[1];
            Projectile.velocity.Y *= 1.005f + (Projectile.ai[0] / 1000);
        }
    }
}
