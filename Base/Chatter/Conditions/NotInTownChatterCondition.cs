using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class NotInTownChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.LocalPlayer.townNPCs < 3;
		}
	}
}
