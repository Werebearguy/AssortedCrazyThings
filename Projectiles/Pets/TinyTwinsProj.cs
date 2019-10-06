using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class TinySpazmatismProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Spazmatism");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = -10;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            projectile.width = 30;
            projectile.height = 30;
        }

        public override bool PreAI()
        {
            Player player = projectile.GetOwner();
            player.eater = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.TinyTwins = false;
            }
            if (modPlayer.TinyTwins)
            {
                projectile.timeLeft = 2;
            }

            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<TinyRetinazerProj>() && projectile.owner == Main.projectile[i].owner)
                {
                    AssUtils.DrawTether(spriteBatch, "AssortedCrazyThings/Projectiles/Pets/TinyTwinsProj_Chain", projectile.Center, Main.projectile[i].Center);
                    break;
                }
            }
            return true;
        }

        public override void PostAI()
        {
            Vector2 between = projectile.Center - projectile.GetOwner().Center;
            projectile.rotation = (float)Math.Atan2(between.Y, between.X) + 1.57f;
            projectile.spriteDirection = projectile.direction = -(between.X < 0).ToDirectionInt();
        }
    }

    public class TinyRetinazerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Retinazer");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = -10;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
            projectile.width = 30;
            projectile.height = 30;
        }

        public override bool PreAI()
        {
            Player player = projectile.GetOwner();
            player.zephyrfish = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.TinyTwins = false;
            }
            if (modPlayer.TinyTwins)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            if (projectile.frame > 1) projectile.frame = 0;

            Vector2 between = projectile.Center - projectile.GetOwner().Center;
            projectile.rotation = (float)Math.Atan2(between.Y, between.X) + 1.57f;
            projectile.spriteDirection = projectile.direction = -(between.X < 0).ToDirectionInt();
        }
    }
}
