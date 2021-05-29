using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class MiniMegalodon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Megalodon");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -4;
            DrawOriginOffsetY = -8;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EyeSpring);
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 24;
            //AIType = ProjectileID.EyeSpring;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.MiniMegalodon = false;
            }
            if (modPlayer.MiniMegalodon)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.EyeSpringAI(Projectile, flyForever: false);
        }
    }
}
