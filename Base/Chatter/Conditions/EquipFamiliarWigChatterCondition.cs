using AssortedCrazyThings.Base.Data;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class EquipFamiliarWigChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.FamiliarWig));
		}
	}
}
