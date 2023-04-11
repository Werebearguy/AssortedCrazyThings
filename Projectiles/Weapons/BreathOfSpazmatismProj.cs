using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Weapons
{
	[Content(ContentType.Weapons)]
	public class BreathOfSpazmatismProj : AssProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spazmatism Breath");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.EyeFire);
			Projectile.aiStyle = 23;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			//adding this makes the game select the AI that is meant for the Spazmatism cursed fire ai
			AIType = ProjectileID.EyeFire;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Main.rand.NextFloat() >= .66f)
			{
				target.AddBuff(BuffID.CursedInferno, 180); //three full seconds, 66% chance
			}
		}
	}
}
