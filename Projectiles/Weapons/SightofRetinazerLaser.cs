using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Weapons)]
	public class SightofRetinazerLaser : AssProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MiniRetinaLaser;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Laser");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.MiniRetinaLaser);
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.MiniRetinaLaser;

			Projectile.usesIDStaticNPCImmunity = false;
			Projectile.idStaticNPCHitCooldown = -1;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 6;
		}
	}
}
