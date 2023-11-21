using Terraria.ID;
using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class GoblinArmyInvasionOngoingChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.invasionType == InvasionID.GoblinArmy;
		}
	}
}
