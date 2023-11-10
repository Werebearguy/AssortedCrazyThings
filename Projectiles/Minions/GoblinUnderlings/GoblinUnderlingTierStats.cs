namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
#pragma warning disable IDE1006 // Naming Styles
	public record struct GoblinUnderlingTierStats(int rangedProjType, float damageMult = 1f, float knockbackMult = 1f, int armorPen = 0, float movementSpeedMult = 1f, int meleeAttackInterval = 4, int meleeAttackHitboxIncrease = 0, float rangedAttackIntervalMultiplier = 1.5f, float rangedVelocity = 10f, float rangedRangeMultiplier = 1f, bool showMeleeDuringRanged = false, bool rangedOnly = false)
#pragma warning restore IDE1006 // Naming Styles
	{
		
	}
}
