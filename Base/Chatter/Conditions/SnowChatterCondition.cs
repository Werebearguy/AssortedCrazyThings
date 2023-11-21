using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SnowChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.LocalPlayer.ZoneSnow;
		}
	}
}
