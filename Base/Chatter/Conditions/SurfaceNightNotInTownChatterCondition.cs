using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SurfaceNightNotInTownChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return !Main.dayTime && Main.LocalPlayer.position.Y / 16 < Main.worldSurface && Main.LocalPlayer.townNPCs < 3;
		}
	}
}
