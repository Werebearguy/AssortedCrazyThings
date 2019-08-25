using AssortedCrazyThings.Base;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimePink : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Pink Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimePinkNew");
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Overworld;
            }
        }

        public override void MoreSetDefaults()
        {
            npc.scale = 0.5f;
            drawOffsetY = -2f;
        }
    }
}
