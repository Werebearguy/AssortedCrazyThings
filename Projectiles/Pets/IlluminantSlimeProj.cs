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
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            DrawOffsetX = -10;
            DrawOriginOffsetY = -4;
        }

        public override void SafeSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            Projectile.width = 32;
            Projectile.height = 30;
            Projectile.alpha = 80;

            Projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (Projectile.GetOwner().dead)
            {
                modPlayer.IlluminantSlime = false;
            }
            if (modPlayer.IlluminantSlime)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }

        public override void PostDraw(Color drawColor)
        {
            Texture2D image = Mod.GetTexture("Projectiles/Pets/IlluminantSlimeProj_Glowmask").Value;
            Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2((Projectile.width - DrawOffsetX) * 0.5f - 5, Projectile.height * 0.5f + Projectile.gfxOffY);
            //the higher the k, the older the position
            //Length is implicitely set in TrailCacheLength up there
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - k) / (1f * Projectile.oldPos.Length)) * ((255 - Projectile.alpha) / 255f);
                color.A = (byte)(Projectile.alpha * ((Projectile.oldPos.Length - k) / (1f * Projectile.oldPos.Length)));
                Main.EntitySpriteDraw(image, drawPos, bounds, color, Projectile.oldRot[k], bounds.Size() / 2, Projectile.scale, effect, 0);
            }
        }
    }
}
