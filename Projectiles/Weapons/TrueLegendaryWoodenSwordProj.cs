using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Weapons)]
	public class TrueLegendaryWoodenSwordProj : AssProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Legendary Wooden Sword");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			Projectile.aiStyle = 2; //6 for powder, 2 for throwing knife
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.alpha = 255;
			//Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;

			//DrawOriginOffsetX = 0;
			//DrawOffsetX = (int)0;
			DrawOriginOffsetX = -(Projectile.width / 2 - 60f / 2);
			DrawOffsetX = (int)-DrawOriginOffsetX * 2;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Vector2.Normalize(Projectile.velocity) * 30f, 50, 50, 169, Projectile.velocity.X, Projectile.velocity.Y, 100, Color.White, 1.25f);
				dust.noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			return true;
		}

		public override void PostAI()
		{
			if (Projectile.ai[0] < 15) Projectile.ai[0] = 15;
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}
			else
			{
				//162 for "sparks"
				//169 for just light
				int dustType = 169;
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Hitbox.X, Projectile.Hitbox.Y) - Vector2.Normalize(Projectile.velocity) * 40f, Projectile.Hitbox.Width, Projectile.Hitbox.Height, dustType, Projectile.velocity.X, Projectile.velocity.Y, 100, Color.White, 1.25f);
				dust.noGravity = true;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 0.785f;
			//projectile.rotation = 0;
		}
	}
}
