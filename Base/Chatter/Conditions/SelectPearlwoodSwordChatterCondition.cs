using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SelectPearlwoodSwordChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return JustSelectedItem(param, ItemID.PearlwoodSword);
		}
	}
}
