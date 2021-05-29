using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class ObservingEyeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Observing Eye");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);
            AIType = ProjectileID.ZephyrFish;
            Projectile.width = 30;
            Projectile.height = 48;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.zephyrfish = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.ObservingEye = false;
            }
            if (modPlayer.ObservingEye)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            if (Projectile.frame > 1) Projectile.frame = 0;

            Vector2 between = Projectile.Center - Projectile.GetOwner().Center;
            Projectile.rotation = (float)Math.Atan2(between.Y, between.X) + 1.57f;
            Projectile.spriteDirection = Projectile.direction = -(between.X < 0).ToDirectionInt();
        }
    }
}
