using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Weapons;
using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	[LocalizeEnum]
	public enum GoblinUnderlingClass : byte
	{
		Melee = 0,
		Magic = 1,
		Ranged = 2,
	}

	/// <summary>
	/// Dps: Round(<see cref="EagerUnderlingItem.BaseDmg"/> * <see cref="damageMult"/> + <see cref="armorPen"/> / 2) * (60 / (attackInterval * <see cref="EagerUnderlingProj.WeaponFrameCount"/>))
	/// </summary>
	public class GoblinUnderlingTierStats
	{
		public readonly int rangedProjType;
		public readonly float damageMult = 1f;
		public readonly float knockbackMult = 1f;
		public readonly int armorPen = 0;
		public readonly float movementSpeedMult = 1f;
		/// <summary>
		/// Multiplied by <see cref="EagerUnderlingProj.WeaponFrameCount"/> for full attack delay/interval
		/// </summary>
		public readonly int attackInterval = 4;
		public readonly float rangedVelocity = 10f;
		public readonly float rangedRangeMultiplier = 1f;
		public readonly float gravity = -1f;
		public readonly int ticksWithoutGravity = 0;
		public readonly Vector2 projOffset = Vector2.Zero;
		public readonly int shootFrame = 1;

		public GoblinUnderlingTierStats(int rangedProjType,
			float damageMult = 1f,
			float knockbackMult = 1f,
			int armorPen = 0,
			float movementSpeedMult = 1f,
			int attackInterval = 4,
			float rangedVelocity = 10,
			float rangedRangeMultiplier = 1f,
			float gravity = 1f,
			int ticksWithoutGravity = 0,
			Vector2 projOffset = default,
			int shootFrame = 1)
		{
			this.rangedProjType = rangedProjType;
			this.damageMult = damageMult;
			this.knockbackMult = knockbackMult;
			this.armorPen = armorPen;
			this.movementSpeedMult = movementSpeedMult;
			this.attackInterval = attackInterval;
			this.rangedVelocity = rangedVelocity;
			this.rangedRangeMultiplier = rangedRangeMultiplier;
			this.gravity = gravity;
			this.ticksWithoutGravity = ticksWithoutGravity;
			this.projOffset = projOffset;
			this.shootFrame = shootFrame;
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
			float gravity = GoblinUnderlingWeaponDart.Gravity,
			int ticksWithoutGravity = GoblinUnderlingWeaponDart.TicksWithoutGravity,
			Vector2 projOffset = default,
			bool showMeleeDuringRanged = false,
			bool rangedOnly = false) : base(rangedProjType, damageMult, knockbackMult, armorPen, movementSpeedMult, attackInterval, rangedVelocity, rangedRangeMultiplier, gravity, ticksWithoutGravity, projOffset, 1)
		{
			this.meleeAttackHitboxIncrease = meleeAttackHitboxIncrease;
			this.rangedAttackIntervalMultiplier = rangedAttackIntervalMultiplier;
			this.showMeleeDuringRanged = showMeleeDuringRanged;
			this.rangedOnly = rangedOnly;
		}
	}

	public class GoblinUnderlingRangedTierStats : GoblinUnderlingTierStats
	{
		public GoblinUnderlingRangedTierStats(int rangedProjType,
			float damageMult = 1f,
			float knockbackMult = 1f,
			int armorPen = 0,
			float movementSpeedMult = 1f,
			int attackInterval = 4,
			float rangedVelocity = 10f,
			float rangedRangeMultiplier = 1f,
			float gravity = GoblinUnderlingWeaponArrow.Gravity,
			int ticksWithoutGravity = GoblinUnderlingWeaponArrow.TicksWithoutGravity) : base(rangedProjType, damageMult, knockbackMult, armorPen, movementSpeedMult, attackInterval, rangedVelocity, rangedRangeMultiplier, gravity, ticksWithoutGravity, new Vector2(0, 6), 1)
		{

		}
	}
}
