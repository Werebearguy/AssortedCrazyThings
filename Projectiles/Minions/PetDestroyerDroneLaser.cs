using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions
{
	[Content(ContentType.DroppedPets | ContentType.Weapons, needsAllToFilterOut: true)] //Also used by BasicLaserDrone
	public class PetDestroyerDroneLaser : AssProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MiniRetinaLaser; 

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.MiniRetinaLaser);
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.5f }, Projectile.position);
			return true;
		}

		public override void AI()
		{
			if (Projectile.ai[1] == 0f)
			{
				Projectile.ai[1] = 1f;
				SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.5f }, Projectile.position);
			}

			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 15;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}
	}
}
