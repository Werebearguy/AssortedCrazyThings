using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class PrinceSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prince Slime");
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -10;
            DrawOriginOffsetY = -4;
        }

        public override void SafeSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            Projectile.width = 32;
            Projectile.height = 30;

            Projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            if (Projectile.GetOwner().dead)
            {
                modPlayer.PrinceSlime = false;
            }
            if (modPlayer.PrinceSlime)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }

        public override void PostDraw(Color drawColor)
        {
            Texture2D image = Mod.GetTexture("Projectiles/Pets/PrinceSlimeProj_Glowmask").Value;
            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = Projectile.frame,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 6
            };
            bounds.Y *= bounds.Height; //cause proj.frame only contains the frame number

            Vector2 stupidOffset = new Vector2(0f, Projectile.gfxOffY - DrawOriginOffsetY); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f + DrawOriginOffsetY);
            Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

            drawColor.A = 255;

            Main.spriteBatch.Draw(image, drawPos, bounds, drawColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effect, 0f);
        }
    }
}
