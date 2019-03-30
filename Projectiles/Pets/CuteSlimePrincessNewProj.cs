using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class CuteSlimePrincessNewProj : CuteSlimeBaseProj
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/CuteSlimeBlueNewProj"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Blue Slime");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -18;
            //drawOriginOffsetX = 0;
            drawOriginOffsetY = -16; //-20
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PetLizard);
            projectile.width = Projwidth; //64 because of wings
            projectile.height = Projheight;
            aiType = ProjectileID.PetLizard;
            projectile.alpha = 75;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.CuteSlimePrincessNew = false;
            }
            if (modPlayer.CuteSlimePrincessNew)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void MorePostDrawBaseSprite(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D image = mod.GetTexture("Projectiles/Pets/CuteSlimePrincessNewProjAddition");
            Rectangle frameLocal = new Rectangle(0, frame2 * image.Height / 10, image.Width, image.Height / 10);
            Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, -6f - drawOriginOffsetY + projectile.gfxOffY);
            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, projectile.rotation, frameLocal.Size() / 2, projectile.scale, effects, 0f);
        }
    }
}
