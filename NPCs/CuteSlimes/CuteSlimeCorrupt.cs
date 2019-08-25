using AssortedCrazyThings.Base;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeCorrupt : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Corrupt Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeCorruptNew");
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Corruption;
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

        public override void MoreSetDefaults()
        {
            npc.scale = 1.2f;
        }
    }
}
