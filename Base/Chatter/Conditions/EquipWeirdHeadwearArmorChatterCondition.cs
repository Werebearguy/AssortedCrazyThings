using AssortedCrazyThings.Base.Data;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class EquipWeirdHeadwearArmorChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.EmptyBucket)) ||
				JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.GoldGoldfishBowl)) ||
				JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.FishBowl));
		}
	}
}
