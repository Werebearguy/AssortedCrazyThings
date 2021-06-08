using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeLavaProj : CuteSlimeBaseProj
    {
        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeLava;

        public override void SafeSetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Lava Slime");
        }

        public override void SafeSetDefaults()
        {
            Projectile.alpha = 75;
        }

        public override void AI()
        {
            Vector2 pos = Projectile.BottomLeft - new Vector2(0f, Projheight);
            Dust.NewDustDirect(pos, 28, Projheight, 6).noGravity = true;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor = Color.White;
            drawColor.A = 75;
            return drawColor;
        }

        public override void SafePostDrawBaseSprite(Color lightColor, bool useNoHair)
        {
            var asset = SheetAdditionAssets[Projectile.type];
            if (asset == null)
            {
                return;
            }

            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = asset.Value;
            Rectangle frameLocal = image.Frame(SheetCountX, SheetCountY, frameX, frameY);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, -6f - DrawOriginOffsetY + Projectile.gfxOffY);
            Main.EntitySpriteDraw(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects, 0);
        }
    }
}
