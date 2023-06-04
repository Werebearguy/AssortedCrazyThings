namespace AssortedCrazyThings.Projectiles.Tools
{
	public class ExtendoNetGoldenProj : ExtendoNetBaseProj
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.scale = 1.3f;
			rangeMin = 5 * 16f;
			rangeMax = 14 * 16f;
		}

		public override bool? CanCutTiles()
		{
			return true;
		}
	}
}
