using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class CrimsonChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.LocalPlayer.ZoneCrimson;
		}
	}
}
