using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    [Content(ContentType.DroppedPets)]
    public class FairySwarmProj : AssProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fairy Swarm");
            Main.projFrames[Projectile.type] = 1; //The texture is a dummy
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
                modPlayer.FairySwarm = false;
            }
            if (modPlayer.FairySwarm)
            {
                Projectile.timeLeft = 2;
            }

            Projectile.Center = player.Center;
            Projectile.gfxOffY = player.gfxOffY;
        }
    }
}
