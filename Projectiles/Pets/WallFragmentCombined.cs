using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class WallFragmentProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyEater);
            AIType = ProjectileID.BabyEater;
            Projectile.width = 26;
            Projectile.height = 40;
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
                modPlayer.WallFragment = false;
            }
            if (modPlayer.WallFragment)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }
    }

    public class WallFragmentEye1 : WallFragmentProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Eye1");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }

    }

    public class WallFragmentEye2 : WallFragmentProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Eye2");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }
    }

    public class WallFragmentMouth : WallFragmentProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Mouth");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }
    }
}
