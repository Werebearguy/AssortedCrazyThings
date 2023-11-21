using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Shy
{
	public class ShyUnderlingProj : GoblinUnderlingProj
	{
		public override string FolderName => "Shy";

		public override GoblinUnderlingChatterType ChatterType => GoblinUnderlingChatterType.Shy;

		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Ranged;
		}
	}
}
