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
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            projectile.width = 26;
            projectile.height = 40;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.WallFragment = false;
            }
            if (modPlayer.WallFragment)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }
    }

    public class WallFragmentEye1 : WallFragmentProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Eye1");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

    }

    public class WallFragmentEye2 : WallFragmentProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Eye2");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }
    }

    public class WallFragmentMouth : WallFragmentProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Mouth");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }
    }
}
