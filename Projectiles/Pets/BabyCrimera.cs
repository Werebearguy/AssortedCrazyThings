using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class BabyCrimera : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Crimera");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
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
                modPlayer.BabyCrimera = false;
            }
            if (modPlayer.BabyCrimera)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }
    }
}
