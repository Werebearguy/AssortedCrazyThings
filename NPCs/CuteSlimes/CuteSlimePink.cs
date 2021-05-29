using AssortedCrazyThings.Base;
using Terraria.ModLoader;

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
                return Mod.Find<ModItem>("CuteSlimePinkNew").Type;
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
            NPC.scale = 0.5f;
            DrawOffsetY = -2f;
        }
    }
}
