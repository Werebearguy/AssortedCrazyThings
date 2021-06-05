using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeRainbowProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeRainbow;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Rainbow Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.scale = 1.2f;
            Projectile.alpha = 75;
            DrawOriginOffsetY = -14;
        }

        public override bool MorePreDrawBaseSprite(Color lightColor, bool useNoHair)
        {
            double cX = Projectile.position.X + Projwidth * 2 + DrawOffsetX;
            double cY = Projectile.position.Y + (Projheight - (DrawOriginOffsetY + 20f)) * 2;  //20f for offset pre-draw, idk how and why
            lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);
            lightColor *= (255f - Projectile.alpha) / 255f;
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = (useNoHair ? SheetNoHairAssets : SheetAssets)[Projectile.type].Value;
            Rectangle frameLocal = image.Frame(SheetCountX, SheetCountY, frameX, frameY);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, 6f + DrawOriginOffsetY + 20f + Projectile.gfxOffY); //20f for offset pre-draw, idk how and why
            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects, 0f);
            return false;
        }
    }
}
