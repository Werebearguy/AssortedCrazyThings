namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	public enum GoblinUnderlingClass
	{
		Melee,
	}

	public abstract class GoblinUnderlingTierStats
	{
		public readonly int rangedProjType;
		public readonly float damageMult = 1f;
		public readonly float knockbackMult = 1f;
		public readonly int armorPen = 0;
		public readonly float movementSpeedMult = 1f;
		public readonly int attackInterval = 4;
		public readonly float rangedRangeMultiplier = 1f;
		public readonly float rangedVelocity = 10f;

		public GoblinUnderlingTierStats(int rangedProjType,
			float damageMult = 1f,
			float knockbackMult = 1f,
			int armorPen = 0,
			float movementSpeedMult = 1f,
			int attackInterval = 4,
			float rangedRangeMultiplier = 1f,
			float rangedVelocity = 10)
		{
			this.rangedProjType = rangedProjType;
			this.damageMult = damageMult;
			this.knockbackMult = knockbackMult;
			this.armorPen = armorPen;
			this.movementSpeedMult = movementSpeedMult;
			this.attackInterval = attackInterval;
			this.rangedRangeMultiplier = rangedRangeMultiplier;
			this.rangedVelocity = rangedVelocity;
		}
	}

	public class GoblinUnderlingMeleeTierStats : GoblinUnderlingTierStats
	{
		public readonly int meleeAttackHitboxIncrease = 0;
		public readonly float rangedAttackIntervalMultiplier = 1.5f;
		public readonly bool showMeleeDuringRanged = false;
		public readonly bool rangedOnly = false;

		public GoblinUnderlingMeleeTierStats(int rangedProjType,
			float damageMult = 1f,
			float knockbackMult = 1f,
			int armorPen = 0,
			float movementSpeedMult = 1f,
			int attackInterval = 4,
			int meleeAttackHitboxIncrease = 0,
			float rangedAttackIntervalMultiplier = 1.5f,
			float rangedVelocity = 10f,
			float rangedRangeMultiplier = 1f,
			bool showMeleeDuringRanged = false,
			bool rangedOnly = false) : base(rangedProjType, damageMult, knockbackMult, armorPen, movementSpeedMult, attackInterval, rangedRangeMultiplier, rangedVelocity)
		{
			this.meleeAttackHitboxIncrease = meleeAttackHitboxIncrease;
			this.rangedAttackIntervalMultiplier = rangedAttackIntervalMultiplier;
			this.showMeleeDuringRanged = showMeleeDuringRanged;
			this.rangedOnly = rangedOnly;
		}
	}
}
