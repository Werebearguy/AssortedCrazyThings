using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    [Autoload]
    public class HeavyLaserDroneLaser : AssProjectile
    {
        //shot with magnitude 6
        private static readonly int LifeTime = 100;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Empty";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Drone Laser");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = LifeTime;
            Projectile.extraUpdates = LifeTime;
            Projectile.hide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255; //might not be needed
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item67, Projectile.position); //67, 94
                //75 for a weak laser
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                for (int i = 0; i < 4; i++) //3
                {
                    Vector2 pos = Projectile.position;
                    pos -= Projectile.velocity * (i * 0.25f); //0.3333
                    int type = 60; //173
                    Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, type, 0f, 0f, 0, default(Color), 1f);
                    //dust.position = pos;
                    dust.noGravity = true;
                    dust.scale = Main.rand.Next(70, 110) * 0.015f;
                    dust.velocity *= 0.2f;
                }
            }
        }
    }
}
