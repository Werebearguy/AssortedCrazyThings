using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeIlluminantNewProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Illuminant Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -16; //-20
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.alpha = 80;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeIlluminantNew = false;
            }
            if (modPlayer.CuteSlimeIlluminantNew)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void MorePostDrawBaseSprite(SpriteBatch spriteBatch, Color drawColor, bool useNoHair)
        {
            Texture2D image = mod.GetTexture("Projectiles/Pets/CuteSlimeIlluminantNewProjAddition" + (useNoHair? "NoHair": ""));

            Rectangle bounds = new Rectangle
            {
                X = 0,
                Y = frame2,
                Width = image.Bounds.Width,
                Height = image.Bounds.Height / 10
            };
            bounds.Y *= bounds.Height;

            SpriteEffects effect = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(-drawOffsetX - 4f, projectile.gfxOffY - drawOriginOffsetY / 2 + 2f);
            //the higher the k, the older the position
            //Length is implicitely set in TrailCacheLength up there
            for (int k = projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                Color color = projectile.GetAlpha(Color.White) * ((projectile.oldPos.Length - k) / (1f * projectile.oldPos.Length)) * ((255 - projectile.alpha) / 255f) * 0.5f;
                color.A = (byte)(projectile.alpha * ((projectile.oldPos.Length - k) / projectile.oldPos.Length));
                spriteBatch.Draw(image, drawPos, bounds, color, projectile.oldRot[k], bounds.Size() / 2, projectile.scale, effect, 0f);
            }
        }
    }
}
