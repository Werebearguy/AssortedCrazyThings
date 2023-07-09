using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SurfaceNoLightChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.LocalPlayer.position.Y / 16 < Main.worldSurface && Lighting.GetColor(Main.LocalPlayer.Center.ToTileCoordinates()).GetAverage() < 255 * 0.1f;
		}
	}
}
