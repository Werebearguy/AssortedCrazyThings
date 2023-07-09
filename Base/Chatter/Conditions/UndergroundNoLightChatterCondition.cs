using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class UndergroundNoLightChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return (Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight) && Lighting.GetColor(Main.LocalPlayer.Center.ToTileCoordinates()).GetAverage() < 255 * 0.1f;
		}
	}
}
