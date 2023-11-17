using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public abstract class CurrentClassCondition : ChatterCondition
	{
		public GoblinUnderlingClass Class { get; init; }

		public CurrentClassCondition(GoblinUnderlingClass @class)
		{
			Class = @class;
		}

		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (param is not AttackingChatterParams p)
			{
				return false;
			}

			if (p.Projectile.ModProjectile is not GoblinUnderlingProj goblin)
			{
				return false;
			}

			return goblin.currentClass == Class;
		}
	}

	public class MeleeClassCondition : CurrentClassCondition
	{
		public MeleeClassCondition() : base(GoblinUnderlingClass.Melee)
		{

		}

		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (param is not AttackingChatterParams p)
			{
				return false;
			}

			if (p.Projectile.ModProjectile is not GoblinUnderlingProj goblin)
			{
				return false;
			}

			return goblin.MeleeAttacking && base.Check(source, param);
		}
	}

	public class RangedClassCondition : CurrentClassCondition
	{
		public RangedClassCondition() : base(GoblinUnderlingClass.Ranged)
		{

		}
	}

	public class MagicClassCondition : CurrentClassCondition
	{
		public MagicClassCondition() : base(GoblinUnderlingClass.Magic)
		{

		}
	}
}
