using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class MiniMegalodon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Megalodon");
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -4;
            drawOriginOffsetY = -8;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.EyeSpring);
            projectile.aiStyle = -1;
            projectile.width = 32;
            projectile.height = 24;
            //aiType = ProjectileID.EyeSpring;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.MiniMegalodon = false;
            }
            if (modPlayer.MiniMegalodon)
            {
                projectile.timeLeft = 2;
            }
            AssAI.EyeSpringAI(projectile, flyForever: false);
        }
    }
}
