using AssortedCrazyThings.Base.Data;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class EquipAnyMartianArmorChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			bool ret = JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.MartianCostumeMask, ArmorIDs.Body.MartianCostumeShirt, ArmorIDs.Legs.MartianCostumePants)) ||
				JustEquipped(param, new EquipSnapshot(ArmorIDs.Head.MartianUniformHelmet, ArmorIDs.Body.MartianUniformTorso, ArmorIDs.Legs.MartianUniformPants));

			if (!ret && ModLoader.TryGetMod("ThoriumMod", out var mod))
			{
				if (EquipLoader.GetEquipSlot(mod, "ConduitHelmet", EquipType.Head) is int head && head > 0 &&
					EquipLoader.GetEquipSlot(mod, "ConduitSuit", EquipType.Body) is int body && body > 0 &&
					EquipLoader.GetEquipSlot(mod, "ConduitLeggings", EquipType.Legs) is int legs && legs > 0)
				{
					return JustEquipped(param, new EquipSnapshot(head, body, legs));
				}
			}

			return ret;
		}
	}
}
