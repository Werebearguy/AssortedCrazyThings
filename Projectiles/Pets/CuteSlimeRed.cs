using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeRed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Red Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -20;
            drawOriginOffsetX = 0;
            drawOriginOffsetY = -21;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            aiType = ProjectileID.PetLizard;
            projectile.scale = 1.025f;
            projectile.alpha = 75;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeRed = false;
            }
            if (modPlayer.CuteSlimeRed)
            {
                projectile.timeLeft = 2;
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor.R = Math.Min(drawColor.R, (byte)160);
            drawColor.G = Math.Min(drawColor.G, (byte)160);
            drawColor.B = Math.Min(drawColor.B, (byte)160);
            drawColor.A = 150;
            return drawColor;
        }
    }
}
