using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
    public class CuteSlimeLavaNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Lava Slime");
            Main.projFrames[Projectile.type] = 10;
            Main.projPet[Projectile.type] = true;
            DrawOffsetX = -18;
            //DrawOriginOffsetX = 0;
            DrawOriginOffsetY = -16; //-20
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PetLizard);
            Projectile.width = Projwidth; //64 because of wings
            Projectile.height = Projheight;
            AIType = ProjectileID.PetLizard;
            Projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Projectile.GetOwner();
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
            if (player.dead)
            {
                modPlayer.CuteSlimeLavaNew = false;
            }
            if (modPlayer.CuteSlimeLavaNew)
            {
                Projectile.timeLeft = 2;
            }

            int height = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type] - 4;
            Vector2 pos = Projectile.BottomLeft - new Vector2(0f, height);
            Dust.NewDustDirect(pos, 28, height, 6).noGravity = true;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor = Color.White;
            drawColor.A = 75;
            return drawColor;
        }

        public override void MorePostDrawBaseSprite(Color lightColor, bool useNoHair)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = Mod.GetTexture("Projectiles/Pets/CuteSlimes/CuteSlimeLavaNewProjAddition").Value;
            Rectangle frameLocal = new Rectangle(0, frame2 * image.Height / 10, image.Width, image.Height / 10);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, -6f - DrawOriginOffsetY + Projectile.gfxOffY);
            Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects, 0f);
        }
    }
}
