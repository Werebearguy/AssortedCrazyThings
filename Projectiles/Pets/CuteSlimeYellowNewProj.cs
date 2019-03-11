using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeYellowNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Yellow Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -14; //-18
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 1.2f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeYellowNew = false;
            }
            if (modPlayer.CuteSlimeYellowNew)
            {
                projectile.timeLeft = 2;
            }
        }
    }
}
