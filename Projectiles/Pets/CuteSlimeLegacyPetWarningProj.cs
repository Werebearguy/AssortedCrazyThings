using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeLegacyPetWarningProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warning");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = 2;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            aiType = ProjectileID.PetLizard;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.lizard = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.LegacyPet = false;
            }
            if (modPlayer.LegacyPet)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            projectile.rotation = 0;
            projectile.spriteDirection = 1;
        }
    }
}
