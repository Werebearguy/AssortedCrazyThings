using Terraria.ID;

namespace AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls
{
	public class CompanionDungeonSoulPreWOFMinion : CompanionDungeonSoulMinionBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}

		public override void SafeSetDefaults()
		{
			defdistanceFromTarget = 700f;
			defdistancePlayerFarAway = 800f;
			defdistancePlayerFarAwayWhenHasTarget = 1200f;
			defdistanceToEnemyBeforeCanDash = 20f; //20f
			defplayerFloatHeight = -60f; //-60f
			defplayerCatchUpIdle = 300f; //300f
			defbackToIdleFromNoclipping = 150f; //150f
			defdashDelay = 40f; //time it stays in the "dashing" state after a dash, he dashes when he is in state 0 aswell
			defstartDashRange = defdistanceToEnemyBeforeCanDash + 10f; //30f
			defdashIntensity = 4f; //4f

			veloFactorToEnemy = 6f; //8f
			accFactorToEnemy = 16f; //41f

			veloFactorAfterDash = 8f; //4f
			accFactorAfterDash = 41f; //41f

			defveloIdle = 1f;
			defveloCatchUpIdle = 8f;
			defveloNoclip = 12f;

			dustColor = 0;

			//For the prewof version specifically, since you can only summon them and in pairs of 2, they shouldn't steal eachothers iframes
			Projectile.usesIDStaticNPCImmunity = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
	}
}
