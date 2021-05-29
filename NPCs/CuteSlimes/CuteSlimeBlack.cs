using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeBlack : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Black Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeBlackNew").Type;
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
