using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
    public class HeavyLaserDroneLaser : ModProjectile
    {
        //shot with magnitude 6
        private static readonly int LifeTime = 100;

        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Weapons/PocketSand";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Drone Laser");
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = LifeTime;
            projectile.extraUpdates = LifeTime;
            projectile.hide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255; //might not be needed
            projectile.ranged = false;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;

            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Item67, projectile.position); //67, 94
                //75 for a weak laser
            }
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 3f)
            {
                for (int i = 0; i < 4; i++) //3
                {
                    Vector2 pos = projectile.position;
                    pos -= projectile.velocity * (i * 0.25f); //0.3333
                    int type = 60; //173
                    Dust dust = Main.dust[Dust.NewDust(pos, projectile.width, projectile.height, type, 0f, 0f, 0, default(Color), 1f)];
                    //dust.position = pos;
                    dust.noGravity = true;
                    dust.scale = Main.rand.Next(70, 110) * 0.015f;
                    dust.velocity *= 0.2f;
                }
            }
        }
    }
}
