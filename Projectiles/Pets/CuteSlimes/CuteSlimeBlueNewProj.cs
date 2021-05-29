using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeBlueNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Slime");
            Main.projFrames[Projectile.type] = 10;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-20
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PetLizard);
            Projectile.width = Projwidth; //64 because of wings
            Projectile.height = Projheight;
            AIType = ProjectileID.PetLizard;
            Projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.CuteSlimeBlueNew = false;
            }
            if (modPlayer.CuteSlimeBlueNew)
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}
