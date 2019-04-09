using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class SkeletronHandProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Pet Hand");
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
            drawOriginOffsetY = -8;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyEater);
            aiType = ProjectileID.BabyEater;
            //projectile.aiStyle = -1;
            projectile.width = 30;
            projectile.height = 32;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.eater = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.SkeletronHand = false;
            }
            if (modPlayer.SkeletronHand)
            {
                projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            AssUtils.DrawSkeletronLikeArms(spriteBatch, "AssortedCrazyThings/Projectiles/Pets/SkeletronHand_Arm", projectile.Center, Main.player[projectile.owner].Center, selfPad: 6f, centerPad: -20f, direction: 0);
            return true;
        }

        public override void PostAI()
        {
            //AssAI.ZephyrfishAI(projectile, velocityFactor: 1f, sway: 2, swapSides: 0, offsetX: 90, offsetY: -10);
            //AssAI.ZephyrfishDraw(projectile);
            projectile.rotation = projectile.velocity.X * 0.08f;
        }
    }
}
