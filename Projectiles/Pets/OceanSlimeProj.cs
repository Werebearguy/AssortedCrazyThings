using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class OceanSlimeProj : BabySlimeBase
    {
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Projectiles/Pets/OceanSlimeProj_0";
			}
		}
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ocean Slime");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -6;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 34;
            projectile.height = 30;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
            if (Main.player[projectile.owner].dead)
            {
                modPlayer.OceanSlime = false;
            }
            if (modPlayer.OceanSlime)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			PetPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>(mod);
			SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D image = mod.GetTexture("Projectiles/Pets/OceanSlimeProj_" + mPlayer.oceanSlimeType);
			Rectangle bounds = new Rectangle
			{
				X = 0,
				Y = projectile.frame,
				Width = image.Bounds.Width,
				Height = image.Bounds.Height / Main.projFrames[projectile.type]
			};
			bounds.Y *= bounds.Height;

			Vector2 stupidOffset = new Vector2(projectile.width / 2, projectile.height / 2 + projectile.gfxOffY);

            if (mPlayer.oceanSlimeType == 0)
            {
                lightColor = lightColor * ((255f - projectile.alpha) / 255f);
            }

            spriteBatch.Draw(image, projectile.position - Main.screenPosition + stupidOffset, bounds, lightColor, projectile.rotation, bounds.Size() / 2, projectile.scale, effects, 0f);

			return false;
		}
    }
}
