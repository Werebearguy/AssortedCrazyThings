using AssortedCrazyThings.Base;
using Terraria.ModLoader;

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
                return Mod.Find<ModItem>("CuteSlimeGreenNew").Type;
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
            NPC.scale = 0.9f;
        }
    }
}
