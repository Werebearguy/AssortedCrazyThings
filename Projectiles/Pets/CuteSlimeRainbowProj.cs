using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimeRainbowProj : CuteSlimeBaseProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Rainbow Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            //moved offset to here just like the other slime girls
            drawOffsetX = -18;
            //drawOriginOffsetX = -0;
            drawOriginOffsetY = -14; //-18 //28 //8
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.scale = 1.2f;
            projectile.alpha = 75;
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

        public override bool MorePreDrawBaseSprite(SpriteBatch spriteBatch, Color lightColor, bool useNoHair)
        {
            double cX = projectile.position.X + Projwidth * 2 + drawOffsetX;
            double cY = projectile.position.Y + (Projheight - (drawOriginOffsetY + 20f)) * 2;  //20f for offset pre-draw, idk how and why
            Color baseColor = new Color()
            {
                R = (byte)Main.DiscoR,
                G = (byte)Main.DiscoG,
                B = (byte)Main.DiscoB
            };
            lightColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), baseColor * 1.2f);
            lightColor.A = (byte)(255 - projectile.alpha);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = Main.projectileTexture[projectile.type];
            Rectangle frameLocal = new Rectangle(0, frame2 * Texheight, image.Width, image.Height / 10);
            Vector2 stupidOffset = new Vector2(14f, 4f + drawOriginOffsetY + 20f + projectile.gfxOffY); //20f for offset pre-draw, idk how and why
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, new Rectangle?(frameLocal), lightColor, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effects, 0f);
            return false;
        }
    }
}
