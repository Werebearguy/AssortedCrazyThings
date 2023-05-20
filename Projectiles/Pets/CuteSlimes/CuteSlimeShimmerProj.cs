using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimeShimmerProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeShimmer;

		public override void SafeSetDefaults()
		{
			Projectile.alpha = 0;
		}

		public override void AI()
		{
			//Lighting.AddLight(Projectile.Center, 23); Not a light pet
			if ((Projectile.velocity.LengthSquared() > 1f && Main.rand.NextBool(3)) || Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(Projectile.Hitbox), 306);
				dust.noGravity = true;
				dust.noLightEmittence = true;
				dust.alpha = 127;
				dust.color = Main.hslToRgb(((float)Main.timeForVisualEffects / 300f + Main.rand.NextFloat() * 0.1f) % 1f, 1f, 0.65f);
				dust.color.A = 0;
				dust.velocity = dust.position - Projectile.Center;
				dust.velocity *= 0.1f;
				dust.velocity.X *= 0.25f;
				if (dust.velocity.Y > 0f)
				{
					dust.velocity.Y *= -1f;
				}

				dust.scale = Main.rand.NextFloat() * 0.3f + 0.5f;
				dust.fadeIn = 0.9f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
		}

		public override bool SafePreDrawBaseSprite(Color lightColor, bool useNoHair)
		{
			if (!Projectile.isAPreviewDummy)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
			}

			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D image = (useNoHair ? SheetNoHairAssets : SheetAssets)[Projectile.type].Value;
			Rectangle frameLocal = image.Frame(SheetCountX, SheetCountY, frameX, frameY);
			Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, 10f + Projectile.gfxOffY);

			DrawData data = new DrawData(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, Projectile.GetAlpha(lightColor), Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects);
			GameShaders.Misc["RainbowTownSlime"].Apply(data);
			data.Draw(Main.spriteBatch);
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			if (!Projectile.isAPreviewDummy)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			}

			return false;
		}

		public override void SafePostDrawBaseSprite(Color lightColor, bool useNoHair)
		{
			var asset = SheetAdditionAssets[Projectile.type];
			if (asset == null)
			{
				return;
			}

			int intended = Main.CurrentDrawnEntityShader;
			Main.instance.PrepareDrawnEntityDrawing(Projectile, 0, Projectile.isAPreviewDummy ? Main.UIScaleMatrix : Main.Transform);

			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D image = asset.Value;
			Rectangle frameLocal = image.Frame(SheetCountX, SheetCountY, frameX, frameY);
			Vector2 stupidOffset = new Vector2(Projwidth * 0.5f, -6f - DrawOriginOffsetY + Projectile.gfxOffY);
			Main.spriteBatch.Draw(image, Projectile.position - Main.screenPosition + stupidOffset, frameLocal, lightColor, Projectile.rotation, frameLocal.Size() / 2, Projectile.scale, effects, 0);
			Main.instance.PrepareDrawnEntityDrawing(Projectile, intended, Projectile.isAPreviewDummy ? Main.UIScaleMatrix : Main.Transform);
		}
	}
}
