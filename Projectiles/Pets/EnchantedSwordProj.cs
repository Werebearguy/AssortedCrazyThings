using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class EnchantedSwordProj : SimplePetProjBase
	{
		private static Asset<Texture2D> glowAsset;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				glowAsset ??= ModContent.Request<Texture2D>(Texture + "_Glowmask");
			}
		}

		public override void Unload()
		{
			glowAsset = null;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 1, 5)
				.WithOffset(-6, -15f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.FloatAndRotateForwardWhenWalking);
		}

		private float sinY;
		private float sincounter;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.TikiSpirit);
			AIType = ProjectileID.TikiSpirit;
			Projectile.width = 26;
			Projectile.height = 26;
			DrawOriginOffsetY = -14;
		}

		public override bool PreAI()
		{
			Player player = Projectile.GetOwner();
			player.tiki = false;
			return true;
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.EnchantedSword = false;
			}
			if (modPlayer.EnchantedSword)
			{
				Projectile.timeLeft = 2;
			}
			AssAI.TeleportIfTooFar(Projectile, player.MountedCenter);

			sincounter = sincounter >= 360 ? 0 : sincounter + 1;
			sinY = (float)((Math.Sin((sincounter / 180f) * MathHelper.TwoPi) + 1) * 0.5f); //Between 0 and 1
			sinY = Utils.Remap(sinY - 0.2f, 0f, 0.8f, 0f, 1f); //Stay on 0 longer

			Lighting.AddLight(Projectile.Center, (Color.Lerp(Color.White, Color.Blue, 0.3f) * ((sinY + 2) / 3) * 1f).ToVector3());
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
		}

		public override void PostDraw(Color lightColor)
		{
			Color color = Color.White * Projectile.Opacity;
			color.A = 100;
			color *= Math.Max(0, sinY);

			Texture2D texture = glowAsset.Value;
			DrawLikeVanilla(Projectile, color, texture, frame: texture.Frame());
		}

		public static void DrawLikeVanilla(Projectile projectile, Color color, Texture2D texture = null, Vector2 offset = default, Rectangle? frame = null, Vector2 originOffset = default, float rotationOffset = 0f, float scaleOffset = 0f)
		{
			Texture2D realTexture = texture ?? TextureAssets.Projectile[projectile.type].Value;

			SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			int offsetX = 0;
			int offsetY = 0;
			float originX = (realTexture.Width - projectile.width) * 0.5f + projectile.width * 0.5f;
			ProjectileLoader.DrawOffset(projectile, ref offsetX, ref offsetY, ref originX);
			float x = projectile.position.X - Main.screenPosition.X + originX + offsetX;
			float y = projectile.position.Y - Main.screenPosition.Y + projectile.height / 2 + projectile.gfxOffY;
			Rectangle realFrame = frame ?? realTexture.Frame(1, Main.projFrames[projectile.type], frameY: projectile.frame);
			float rotation = projectile.rotation + rotationOffset;
			Vector2 origin = new Vector2(originX, projectile.height / 2 + offsetY);
			float scale = projectile.scale + scaleOffset;
			Main.EntitySpriteDraw(realTexture, new Vector2(x, y) + offset, realFrame, color, rotation, origin + originOffset, scale, effects, 0);
		}
	}
}
