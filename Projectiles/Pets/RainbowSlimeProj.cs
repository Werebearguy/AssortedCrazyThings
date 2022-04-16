using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets)]
	//check this file for more info vvvvvvvv
	public class RainbowSlimeProj : BabySlimeBase
	{
		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbow Slime");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 30;

			Projectile.minion = false;
		}

		public override bool PreAI()
		{
			PetPlayer modPlayer = Projectile.GetOwner().GetModPlayer<PetPlayer>();
			if (Projectile.GetOwner().dead)
			{
				modPlayer.RainbowSlime = false;
			}
			if (modPlayer.RainbowSlime)
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}

		public override bool PreDraw(ref Color drawColor)
		{
			Texture2D image = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);
			Vector2 stupidOffset = new Vector2(0f, Projectile.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f + Projectile.gfxOffY);
			Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + stupidOffset;

			double cX = Projectile.Center.X + DrawOffsetX;
			double cY = Projectile.Center.Y + DrawOriginOffsetY;
			drawColor = Lighting.GetColor((int)(cX / 16), (int)(cY / 16), Main.DiscoColor * 1.2f);

			Color color = drawColor * ((255 - Projectile.alpha) / 255f);

			Main.EntitySpriteDraw(image, drawPos, bounds, color, Projectile.rotation, bounds.Size() / 2, Projectile.scale, effect, 0);
			return false;
		}
	}
}
