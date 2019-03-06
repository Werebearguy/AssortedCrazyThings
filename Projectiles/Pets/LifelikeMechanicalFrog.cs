using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class LifelikeMechanicalFrog : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lifelike Mechanical Frog");
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = 1;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Bunny);
            aiType = ProjectileID.Bunny;
            projectile.width = 18;
            projectile.height = 20;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.bunny = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.LifelikeMechanicalFrog = false;
            }
            if (modPlayer.LifelikeMechanicalFrog)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostAI()
        {
            PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            Main.projectileTexture[projectile.type] = mod.GetTexture("Projectiles/Pets/LifelikeMechanicalFrog" + (mPlayer.mechFrogCrown ? "Crown" : ""));
        }
    }
}
