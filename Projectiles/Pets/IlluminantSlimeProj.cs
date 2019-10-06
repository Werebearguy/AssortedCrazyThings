using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class IlluminantSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Illuminant Slime");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            drawOffsetX = -10;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;
            projectile.alpha = 80;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (projectile.GetOwner().dead)
            {
                modPlayer.IlluminantSlime = false;
            }
            if (modPlayer.IlluminantSlime)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D image = mod.GetTexture("Projectiles/Pets/IlluminantSlimeProj_Glowmask");
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 6
            };
            bounds.Y *= bounds.Height;

            SpriteEffects effect = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2((projectile.width - drawOffsetX) * 0.5f - 5, projectile.height * 0.5f + projectile.gfxOffY);
            //the higher the k, the older the position
            //Length is implicitely set in TrailCacheLength up there
            for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = projectile.GetAlpha(Color.White) * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length)) * ((255 - projectile.alpha) / 255f);
                color.A = (byte)(projectile.alpha * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length)));
                spriteBatch.Draw(image, drawPos, bounds, color, projectile.oldRot[k], bounds.Size() / 2, projectile.scale, effect, 0f);
            }
        }
    }
}
