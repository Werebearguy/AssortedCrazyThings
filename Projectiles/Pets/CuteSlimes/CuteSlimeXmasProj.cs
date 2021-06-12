using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeXmasProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeXmas;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Christmas Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.alpha = 75;
        }

        public override void SafePostDrawBaseSprite(Color lightColor, bool useNoHair)
        {
            var asset = SheetAdditionAssets[Projectile.type];
            if (asset == null)
            {
                return;
            }

            int intended = Main.CurrentDrawnEntityShader;
            Main.instance.PrepareDrawnEntityDrawing(Projectile, 0);

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = asset.Value;
            Rectangle frameLocal = image.Frame(SheetCountX, SheetCountY, frameX, frameY);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, -6f - DrawOriginOffsetY + Projectile.gfxOffY);
            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects, 0);
            Main.instance.PrepareDrawnEntityDrawing(Projectile, intended);
        }
    }
}
