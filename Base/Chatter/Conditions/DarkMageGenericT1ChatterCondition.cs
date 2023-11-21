using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class DarkMageT1GenericChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return MatchesNPCType(param, NPCID.DD2DarkMageT1);
		}
	}
}
