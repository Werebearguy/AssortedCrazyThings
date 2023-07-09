using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SurfaceNightChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return !Main.dayTime && Main.LocalPlayer.position.Y / 16 < Main.worldSurface;
		}
	}
}
