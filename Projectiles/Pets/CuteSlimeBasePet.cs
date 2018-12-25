using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeBasePet: ModProjectile
    {
        public const int Projwidth = 28;
        public const int Projheight = 52;

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.lizard = false;
            return true;
        }
    }
}
