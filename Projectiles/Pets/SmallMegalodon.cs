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
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -45;
            drawOriginOffsetX = 0;
            drawOriginOffsetY = 0;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.EyeSpring);
            projectile.aiStyle = -1;
            projectile.width = 32;
            projectile.height = 32;
            //aiType = ProjectileID.EyeSpring;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SmallMegalodon = false;
            }
            if (modPlayer.SmallMegalodon)
            {
                projectile.timeLeft = 2;
            }
            AssAI.EyeSpringAI(projectile, flyForever: false);
        }
    }
}
