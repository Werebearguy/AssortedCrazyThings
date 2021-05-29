using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeYellow : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Yellow Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeYellowNew").Type;
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
