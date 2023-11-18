using AssortedCrazyThings.Base.Data;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class EquipAnyMartianArmorChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			//TODO thorium conduit armor?
			return JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.MartianCostumeMask, ArmorIDs.Body.MartianCostumeShirt, ArmorIDs.Legs.MartianCostumePants)) ||
				JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.MartianUniformHelmet, ArmorIDs.Body.MartianUniformTorso, ArmorIDs.Legs.MartianUniformPants));
		}
	}
}
