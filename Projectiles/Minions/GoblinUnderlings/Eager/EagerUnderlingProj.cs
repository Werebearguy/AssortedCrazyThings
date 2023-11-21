using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager
{
	public class EagerUnderlingProj : GoblinUnderlingProj
	{
		public override string FolderName => "Eager";

		public override GoblinUnderlingChatterType ChatterType => GoblinUnderlingChatterType.Eager;

		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Melee;
		}

		public override void PostPickDestinationAndAttack(int attackTarget, int oldAttackTarget)
		{
			//Do a little jump when it goes from no target to target
			if (oldAttackTarget == -1 && IdleOrMoving && Projectile.velocity.Y == 0)
			{
				Projectile.velocity.Y -= 4;
			}
		}
	}
}
