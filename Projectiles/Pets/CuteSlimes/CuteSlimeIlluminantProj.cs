using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeIlluminantProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeIlluminant;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Illuminant Slime");
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-20
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }

        public override void SafeSetDefaults()
        {
            Projectile.alpha = 80;
        }

        public override void MorePostDrawBaseSprite(Color drawColor, bool useNoHair)
        {
            Texture2D image = ModContent.GetTexture(Texture + "Addition" + (useNoHair ? "NoHair" : "")).Value;

            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = frame2,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 10
            };
            bounds.Y *= bounds.Height;

            SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(-DrawOffsetX - 4f, Projectile.gfxOffY - DrawOriginOffsetY / 2 + 2f);
            //the higher the k, the older the position
            //Length is implicitely set in TrailCacheLength up there
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - k) / (1f * Projectile.oldPos.Length)) * ((255 - Projectile.alpha) / 255f) * 0.5f;
                color.A = (byte)(Projectile.alpha * ((Projectile.oldPos.Length - k) / Projectile.oldPos.Length));
                Main.spriteBatch.Draw(image, drawPos, bounds, color, Projectile.oldRot[k], bounds.Size() / 2, Projectile.scale, effect, 0f);
            }
        }
    }
}
