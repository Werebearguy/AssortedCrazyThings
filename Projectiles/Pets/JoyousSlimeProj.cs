using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class JoyousSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Joyous Slime");
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -10;
            DrawOriginOffsetY = -4;
        }

        public override void SafeSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            Projectile.width = 32;
            Projectile.height = 30;

            Projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (Projectile.GetOwner().dead)
            {
                modPlayer.JoyousSlime = false;
            }
            if (modPlayer.JoyousSlime)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
