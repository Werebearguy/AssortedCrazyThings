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
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyHornet);
            AIType = ProjectileID.BabyHornet;
            Projectile.width = 34;
            Projectile.height = 38;
        }

        public override bool PreAI()
        {
            Player player = Projectile.GetOwner();
            player.hornet = false; // Relic from AIType
            return true;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.BabyIchorSticker = false;
            }
            if (modPlayer.BabyIchorSticker)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);
        }
    }
}
