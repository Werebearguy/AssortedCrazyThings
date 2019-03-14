using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class RainbowSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rainbow Slime");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.RainbowSlime = false;
            }
            if (modPlayer.RainbowSlime)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 6
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number
            Vector2 stupidOffset = new Vector2(0f, projectile.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(projectile.width * 0.5f, projectile.height * 0.5f + projectile.gfxOffY);
            Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

            double cX = projectile.Center.X + drawOffsetX;
            double cY = projectile.Center.Y + drawOriginOffsetY;
            drawColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);

            Color color = drawColor * ((255 - projectile.alpha) / 255f);

            spriteBatch.Draw(image, drawPos, bounds, color, projectile.rotation, bounds.Size() / 2, projectile.scale, effect, 0f);
            return false;
        }
    }
}
