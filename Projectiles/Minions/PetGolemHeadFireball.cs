using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions
{
	[Content(ContentType.DroppedPets)]
	public class PetGolemHeadFireball : AssProjectile
	{
		public override string Texture
		{
			get
			{
				return "Terraria/Images/Projectile_" + ProjectileID.Fireball;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pet Golem Head Fireball");
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Fireball);
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			//Main.PlaySound(0, projectile.position);
			//Main.PlaySound(SoundID.Dig, (int)projectile.Center.X, (int)projectile.Center.Y, 0, 0.75f);
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.AddBuff(BuffID.OnFire, 240);
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0f)
			{
				SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
			}
			Projectile.localAI[0] += 1f;
		}
	}
}
