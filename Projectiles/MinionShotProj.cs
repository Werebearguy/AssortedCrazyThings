using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles
{
	/// <summary>
	/// Basic class for copying vanilla projectiles safely without having to set .DamageType = Summon on vanilla projectiles (as this one sets MinionShot).
	/// <br>Some complex projectiles might need adjustments, as this only clones Texture, SetDefaults (by extension AI), and Kill behavior.</br>
	/// </summary>
	public abstract class MinionShotProj : AssProjectile
	{
		public abstract int ClonedType { get; }

		public virtual SoundStyle? SpawnSound => null;

		public virtual bool UseCustomTexture => false;

		private bool spawned = false;

		public override string Texture => UseCustomTexture ? base.Texture : "Terraria/Images/Projectile_" + ClonedType;

		public sealed override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = Main.projFrames[ClonedType];

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public sealed override void SetDefaults()
		{
			Projectile.CloneDefaults(ClonedType);
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 180;

			AIType = ClonedType;

			SafeSetDefaults();
		}

		public virtual void SafeSetDefaults()
		{

		}

		public sealed override void AI()
		{
			if (!spawned)
			{
				spawned = true;

				if (SpawnSound != null)
				{
					SoundEngine.PlaySound(SpawnSound.Value, Projectile.Center);
				}
			}

			SafeAI();
		}

		public virtual void SafeAI()
		{

		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ClonedType;

			return base.PreKill(timeLeft);
		}
	}
}
