using AssortedCrazyThings.Base;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeGreen : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Green Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeGreenNew");
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
            npc.scale = 0.9f;
        }
    }
}
