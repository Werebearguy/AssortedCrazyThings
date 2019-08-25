using AssortedCrazyThings.Base;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeToxic : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Toxic Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeToxicNew");
            }
        }

        public override bool IsFriendly
        {
            get
            {
                return false;
            }
        }

        public override bool ShouldDropRandomItem
        {
            get
            {
                return false;
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Underground;
            }
        }
    }
}
