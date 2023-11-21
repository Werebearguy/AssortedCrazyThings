using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class UndergroundChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.LocalPlayer.ZoneDirtLayerHeight || Main.LocalPlayer.ZoneRockLayerHeight;
		}
	}
}
