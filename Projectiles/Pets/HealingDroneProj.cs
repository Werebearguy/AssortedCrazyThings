using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class HealingDroneProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Drone");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2PetGhost);
            projectile.aiStyle = -1;
            projectile.width = 38;
            projectile.height = 30;
            projectile.alpha = 0;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.HealingDrone = false;
            }
            if (modPlayer.HealingDrone)
            {
                projectile.timeLeft = 2;

                AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, veloXToRotationFactor: 0.5f, offsetX: 30f, offsetY: -20f);
                projectile.spriteDirection = 1;
                projectile.rotation = 0f;
            }
        }
    }
}
