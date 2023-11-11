using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingWeaponDaybreakExplosion : AssProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 52;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 0) * 0.85f;
		}

		public override void OnKill(int timeLeft)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 20, 20, 174, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 255, default, 1.25f);
				dust.noGravity = true;
			}
		}

		public override void PostAI()
		{
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;

				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 3)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= Main.projFrames[Projectile.type])
			{
				Projectile.frame = 0;
				Projectile.Kill();
			}
		}
	}
}
