using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Serious
{
	public class SeriousUnderlingProj : GoblinUnderlingProj
	{
		public override string FolderName => "Serious";

		public override GoblinUnderlingChatterType ChatterType => GoblinUnderlingChatterType.Serious;

		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Magic;
		}
	}
}
