using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimePurpleNewPet : CuteSlimeBasePet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Purple Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -14; //-18
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 1.2f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimePurpleNew = false;
            }
            if (modPlayer.CuteSlimePurpleNew)
            {
                projectile.timeLeft = 2;
            }
        }

        //public override Color? GetAlpha(Color drawColor)
        //{
        //    drawColor.R = Math.Min((byte)(drawColor.R * 0.75f), (byte)175);
        //    drawColor.G = Math.Min((byte)(drawColor.G * 0.75f), (byte)175);
        //    drawColor.B = Math.Min((byte)(drawColor.B * 0.75f), (byte)175);
        //    drawColor.A = 225;
        //    return drawColor;
        //}
    }
}
