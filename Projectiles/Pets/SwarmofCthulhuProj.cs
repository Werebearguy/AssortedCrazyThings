using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Autoload]
    public class SwarmofCthulhuProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swarm of Cthulhu");
            Main.projFrames[Projectile.type] = 1; //Dummy
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyEater);
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.SwarmofCthulhu = false;
            }
            if (modPlayer.SwarmofCthulhu)
            {
                Projectile.timeLeft = 2;
            }

            Projectile.Center = player.Center;
            Projectile.gfxOffY = player.gfxOffY;
        }
    }
}
