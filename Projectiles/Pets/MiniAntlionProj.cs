using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class MiniAntlionProj : BabySlimeBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/MiniAntlionProj_0";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Antlion");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = 2;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 34;
            projectile.alpha = 0;

            flyingFrameSpeed = 3;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (projectile.GetOwner().dead)
            {
                modPlayer.MiniAntlion = false;
            }
            if (modPlayer.MiniAntlion)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            PetPlayer mPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D image = mod.GetTexture("Projectiles/Pets/MiniAntlionProj_" + mPlayer.miniAntlionType);
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / Main.projFrames[projectile.type]
            };
            bounds.Y *= bounds.Height;

            Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + projectile.gfxOffY + drawOriginOffsetY);

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

            return false;
        }
    }
}
