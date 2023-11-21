using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public abstract class CurrentClassChatterCondition : ChatterCondition
	{
		public GoblinUnderlingClass Class { get; init; }

		public CurrentClassChatterCondition(GoblinUnderlingClass @class)
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

	public class MeleeClassChatterCondition : CurrentClassChatterCondition
	{
		public MeleeClassChatterCondition() : base(GoblinUnderlingClass.Melee)
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

	public class RangedClassChatterCondition : CurrentClassChatterCondition
	{
		public RangedClassChatterCondition() : base(GoblinUnderlingClass.Ranged)
		{

		}
	}

	public class MagicClassChatterCondition : CurrentClassChatterCondition
	{
		public MagicClassChatterCondition() : base(GoblinUnderlingClass.Magic)
		{

		}
	}
}
