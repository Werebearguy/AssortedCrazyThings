using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class BabyIchorSticker : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Ichor Sticker");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyHornet);
            aiType = ProjectileID.BabyHornet;
            projectile.width = 34;
            projectile.height = 38;
        }

        public override bool PreAI()
        {
            Player player = projectile.GetOwner();
            player.hornet = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.BabyIchorSticker = false;
            }
            if (modPlayer.BabyIchorSticker)
            {
                projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(projectile, player.MountedCenter);
        }
    }
}
