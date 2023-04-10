using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Bosses)]
	public class BoneCleavingFangProj : AssProjectile
	{
		public ref float Power => ref Projectile.ai[0];

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bone-Cleaving Fang");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.height = 28;
			Projectile.width = 28;
			Projectile.aiStyle = -1;
			Projectile.alpha = 150;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 240;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;

			DrawOffsetX = -6;
			DrawOriginOffsetY = -10;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.Inflate(6, 6);
		}

		public override void PostAI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2((texture2D.Width - Projectile.width) / 2 + Projectile.width / 2, Projectile.height / 2);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(DrawOffsetX, Projectile.gfxOffY);
				Color color = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture2D, drawPos, null, color, Projectile.oldRot[k], drawOrigin - new Vector2(DrawOriginOffsetX, DrawOriginOffsetY), Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;

				SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
			}

			Lighting.AddLight(Projectile.Center, 0.05f, 0.1f, 0.45f);

			for (int i = 0; i < Power * 4; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 150);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.velocity += 2.5f * Projectile.velocity * 0.4f;
				dust.scale = 1.5f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Vector2 normalizedVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for (int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 150);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.velocity += 2.5f * normalizedVelocity;
				dust.scale = 1.5f;
			}
		}
	}
}
