using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeRainbowPet : CuteSlimeBasePet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Rainbow Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            //moved offset to here just like the other slime girls
            drawOffsetX = -20;
            //drawOriginOffsetX = -0;
            drawOriginOffsetY = 28; //-18
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 1.2f;
            projectile.alpha = 0;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimeRainbow = false;
            }
            if (modPlayer.CuteSlimeRainbow)
            {
                projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            double cX = projectile.position.X + projectile.width * 2;
            double cY = projectile.position.Y + (projectile.height - drawOriginOffsetY) * 2;
            Color baseColor = new Color()
            {
                R = (byte)Main.DiscoR,
                G = (byte)Main.DiscoG,
                B = (byte)Main.DiscoB
            };
            lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), baseColor);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.direction != -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle bounds = new Rectangle();
            bounds.X = 0;
            bounds.Width = image.Bounds.Width;
            bounds.Height = (int)(image.Bounds.Height / Main.projFrames[projectile.type]);
            bounds.Y = projectile.frame * bounds.Height;
            Vector2 stupidOffset = new Vector2(12f, 6f + drawOriginOffsetY);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Texture2D texture = mod.GetTexture("Projectiles/Pets/CuteSlimeAccessoryBow");
            //Rectangle frameLocal = new Rectangle(0, 0, texture.Width, texture.Height / 10);
            //frameLocal.Y = projectile.frame * Projheight;
            //Vector2 stupidOffset = new Vector2(-2f, -0.7f + drawOriginOffsetY - 19f); // new Vector2(-0.5f, -7.7f); //rainbow special snowflake
            //SpriteEffects effect = projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            //Vector2 drawOrigin = new Vector2(Projwidth * 0.5f, Projheight * 0.5f);
            //Vector2 drawPos = projectile.position - Main.screenPosition + drawOrigin + stupidOffset;
            //spriteBatch.Draw(texture, drawPos, new Rectangle?(frameLocal), Color.White, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effect, 0f);
        }
    }
}
