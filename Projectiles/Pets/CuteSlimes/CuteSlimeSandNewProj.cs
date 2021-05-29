using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeSandNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Sand Slime");
            Main.projFrames[Projectile.type] = 10;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -20;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-21
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PetLizard);
            Projectile.width = Projwidth; //64 because of wings
            Projectile.height = Projheight;
            AIType = ProjectileID.PetLizard;
            //projectile.scale = 1.025f;
            Projectile.alpha = 45;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.CuteSlimeSandNew = false;
            }
            if (modPlayer.CuteSlimeSandNew)
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}
