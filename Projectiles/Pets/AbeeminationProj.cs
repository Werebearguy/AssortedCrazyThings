using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class AbeeminationProj : BabySlimeBase
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/AbeeminationProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination");
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
                modPlayer.Abeemination = false;
            }
            if (modPlayer.Abeemination)
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), Color.White);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.direction != -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            PetPlayer mPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
            Texture2D image = Mod.GetTexture("Projectiles/Pets/AbeeminationProj_" + mPlayer.abeeminationType).Value;
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = image.Bounds.Height / Main.projFrames[Projectile.type];
            bounds.Y = Projectile.frame * bounds.Height;
            Vector2 stupidOffset = new Vector2(Projectile.width * 0.5f/* + DrawOffsetX * 0.5f*/, Projectile.height * 0.5f + Projectile.gfxOffY);
            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effects, 0f);

            return false;
        }
    }
}
