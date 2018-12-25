using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeBlackPet : CuteSlimeBasePet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Black Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -20;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = 0; //-22
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 0.9f;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeBlack = false;
            }
            if (modPlayer.CuteSlimeBlack)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Texture2D texture = mod.GetTexture("Projectiles/Pets/CuteSlimeAccessoryBow");
            //Rectangle frameLocal = new Rectangle(0, 0, texture.Width, texture.Height / 10);
            //frameLocal.Y = projectile.frame * Projheight;
            //Vector2 stupidOffset = new Vector2(-2f, -0.7f + drawOriginOffsetY); // new Vector2(-0.5f, -7.7f);
            //SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            //Vector2 drawOrigin = new Vector2(Projwidth * 0.5f, Projheight * 0.5f);
            //Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
            //spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), Color.White, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);

            //texture = mod.GetTexture("Projectiles/Pets/CuteSlimeXmasHatBlack");
            //frameLocal = new Rectangle(0, 0, texture.Width, texture.Height / 10);
            //frameLocal.Y = projectile.frame * 52;
            //stupidOffset = new Vector2(-2f, -7.7f);
            //effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            //drawOrigin = new Vector2(28 * 0.5f, 52 * 0.5f);
            //drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
            //spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), Color.White, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            drawColor.R = drawColor.R;
            drawColor.G = drawColor.G;
            drawColor.B = drawColor.B;
            drawColor.A = 180;
            return drawColor;
        }
    }
}
