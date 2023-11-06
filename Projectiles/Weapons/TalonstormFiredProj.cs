using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Bosses)]
	public class TalonstormFiredProj : AssProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.active && Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TalonstormProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}

		public override void AI()
		{
			float destinationX = Projectile.ai[0];
			float destinationY = Projectile.ai[1];
			if (destinationX != 0f && destinationY != 0f)
			{
				bool reachedX = false;
				bool reachedY = false;
				if ((Projectile.velocity.X < 0f && Projectile.Center.X < destinationX) || (Projectile.velocity.X > 0f && Projectile.Center.X > destinationX))
				{
					reachedX = true;
				}
				if ((Projectile.velocity.Y < 0f && Projectile.Center.Y < destinationY) || (Projectile.velocity.Y > 0f && Projectile.Center.Y > destinationY))
				{
					reachedY = true;
				}
				if (reachedX & reachedY)
				{
					Projectile.Kill();
				}
			}
		}
	}
}
