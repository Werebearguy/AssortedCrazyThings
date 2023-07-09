using AssortedCrazyThings.Base.Data;

namespace AssortedCrazyThings.Base.Chatter
{
	public class ArmorEquipChatterParams : IChatterParams
	{
		public EquipSnapshot CurrentEquips { get; init; }
		public EquipSnapshot PrevEquips { get; init; }

		public ArmorEquipChatterParams(EquipSnapshot currentEquips, EquipSnapshot prevEquips)
		{
			CurrentEquips = currentEquips;
			PrevEquips = prevEquips;
		}
	}
}
