using AssortedCrazyThings.Base;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeBlue : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Blue Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeBlueNew");
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Overworld;
            }
        }
    }
}
