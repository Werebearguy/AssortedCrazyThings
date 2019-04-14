using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeLavaNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Lava Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -16; //-20
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeLavaNew = false;
            }
            if (modPlayer.CuteSlimeLavaNew)
            {
                projectile.timeLeft = 2;
            }
            
            int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type] - 4;
            Vector2 pos = projectile.BottomLeft - new Vector2(0f, height);
            Main.dust[Dust.NewDust(pos, 28, height, 6)].noGravity = true;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor = Color.White;
            drawColor.A = 75;
            return drawColor;
        }

        public override void MorePostDrawBaseSprite(SpriteBatch spriteBatch, Color lightColor, bool useNoHair)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = mod.GetTexture("Projectiles/Pets/CuteSlimeLavaNewProjAddition");
            Rectangle frameLocal = new Rectangle(0, frame2 * image.Height / 10, image.Width, image.Height / 10);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, -6f - drawOriginOffsetY + projectile.gfxOffY);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effects, 0f);
        }
    }
}
