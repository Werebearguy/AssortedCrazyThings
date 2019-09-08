using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class ChunkyProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            projectile.width = 22;
            projectile.height = 34;
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
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.ChunkyandMeatball = false;
            }
            if (modPlayer.ChunkyandMeatball)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X < 0).ToDirectionInt();
        }
    }

    public class MeatballProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            projectile.width = 22;
            projectile.height = 34;
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
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.ChunkyandMeatball = false;
            }
            if (modPlayer.ChunkyandMeatball)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X < 0).ToDirectionInt();
        }
    }
}
