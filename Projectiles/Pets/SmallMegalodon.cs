using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class SmallMegalodon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Little Megalodon");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -45;
            DrawOriginOffsetX = 0;
            DrawOriginOffsetY = 0;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EyeSpring);
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 32;
            //AIType = ProjectileID.EyeSpring;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SmallMegalodon = false;
            }
            if (modPlayer.SmallMegalodon)
            {
                Projectile.timeLeft = 2;
            }
            AssAI.EyeSpringAI(Projectile, flyForever: false);
        }
    }
}
