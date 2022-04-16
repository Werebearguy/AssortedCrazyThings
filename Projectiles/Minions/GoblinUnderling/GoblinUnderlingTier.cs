using System;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	public record struct GoblinUnderlingTier
	{
		public int texIndex;
		public Func<bool> condition;
		public float damageMult;
		public float knockbackMult;
		public int armorPen;
		public float movementSpeedMult;
		public int meleeAttackInterval;
		public int meleeAttackHitboxIncrease;
		public float rangedAttackIntervalMultiplier;
		public float rangedVelocity;
		public int rangedProjType;
		public bool showMeleeDuringRanged;

		public GoblinUnderlingTier(int texIndex, Func<bool> condition, int rangedProjType, float damageMult = 1f, float knockbackMult = 1f, int armorPen = 0, float movementSpeedMult = 1f, int meleeAttackInterval = 4, int meleeAttackHitboxIncrease = 0, float rangedAttackIntervalMultiplier = 1.5f, float rangedVelocity = 10f, bool showMeleeDuringRanged = false)
		{
			this.texIndex = texIndex;
			this.condition = condition;
			this.damageMult = damageMult;
			this.knockbackMult = knockbackMult;
			this.armorPen = armorPen;
			this.movementSpeedMult = movementSpeedMult;
			this.meleeAttackInterval = meleeAttackInterval;
			this.meleeAttackHitboxIncrease = meleeAttackHitboxIncrease;
			this.rangedAttackIntervalMultiplier = rangedAttackIntervalMultiplier;
			this.rangedVelocity = rangedVelocity;
			this.rangedProjType = rangedProjType;
			this.showMeleeDuringRanged = showMeleeDuringRanged;
		}
	}
}
