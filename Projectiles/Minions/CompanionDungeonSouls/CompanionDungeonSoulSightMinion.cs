using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls
{
	public class CompanionDungeonSoulSightMinion : CompanionDungeonSoulMinionBase
	{
		public override void SafeSetDefaults()
		{
			Projectile.minionSlots = 1f;
			defdistanceFromTarget = 700f;
			defdistancePlayerFarAway = 800f;
			defdistancePlayerFarAwayWhenHasTarget = 1200f;
			defdistanceToEnemyBeforeCanDash = 20f; //20f
			defplayerFloatHeight = -60f; //-60f
			defplayerCatchUpIdle = 300f; //300f
			defbackToIdleFromNoclipping = 150f; //150f
			defdashDelay = 20f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell
			defdistanceAttackNoclip = defdashDelay * 5f;
			defstartDashRange = defdistanceToEnemyBeforeCanDash + 10f; //30f
			defdashIntensity = 1.5f; //4f

			veloFactorToEnemy = 10f; //8f
			accFactorToEnemy = 8f; //41f

			veloFactorAfterDash = 1f; //4f
			accFactorAfterDash = 16f; //41f

			defveloIdle = 1f;
			defveloCatchUpIdle = 8f;
			defveloNoclip = 12f;

			dustColor = ItemID.BrightGreenDye;

			Projectile.idStaticNPCHitCooldown = 14;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.CursedInferno, 120);
			}
		}
	}
}
