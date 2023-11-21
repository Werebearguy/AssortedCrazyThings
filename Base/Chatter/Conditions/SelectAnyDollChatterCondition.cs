using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class SelectAnyDollChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return JustSelectedItem(param, ItemID.ClothierVoodooDoll) || JustSelectedItem(param, ItemID.GuideVoodooDoll);
		}
	}
}
