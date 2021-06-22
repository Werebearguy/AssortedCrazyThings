using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Content(ContentType.HostileNPCs)]
    public class ChunkyProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyEater);
            AIType = ProjectileID.BabyEater;
            Projectile.width = 22;
            Projectile.height = 34;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.eater = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.ChunkyandMeatball = false;
            }
            if (modPlayer.ChunkyandMeatball)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0).ToDirectionInt();
        }
    }

    [Content(ContentType.HostileNPCs)]
    public class MeatballProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyEater);
            AIType = ProjectileID.BabyEater;
            Projectile.width = 22;
            Projectile.height = 34;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.eater = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.ChunkyandMeatball = false;
            }
            if (modPlayer.ChunkyandMeatball)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }

        public override void PostAI()
        {
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X < 0).ToDirectionInt();
        }
    }
}
