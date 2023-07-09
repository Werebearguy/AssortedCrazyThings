using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class GoblinArmyInvasionChangedChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return JustChangedInvasion(param, InvasionID.GoblinArmy);
		}
	}
}
