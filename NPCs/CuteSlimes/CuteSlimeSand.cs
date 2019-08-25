using AssortedCrazyThings.Base;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeSand : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Sand Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeSandNew");
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
                return SpawnConditionType.Desert;
            }
        }

        public override void MoreSetDefaults()
        {
            npc.alpha = 45;
        }
    }
}
