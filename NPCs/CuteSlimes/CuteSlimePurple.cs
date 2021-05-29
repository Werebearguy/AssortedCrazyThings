using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimePurple : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Purple Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimePurpleNew").Type;
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
            NPC.scale = 1.2f;
        }
    }
}
