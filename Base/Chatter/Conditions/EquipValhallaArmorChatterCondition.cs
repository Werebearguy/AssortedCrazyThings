using AssortedCrazyThings.Base.Data;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class EquipValhallaArmorChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.ValhallaKnight, ArmorIDs.Body.ValhallaKnight, ArmorIDs.Legs.ValhallaKnight));
		}
	}
}
