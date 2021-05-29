using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeJungle : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Jungle Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeJungleNew").Type;
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Jungle;
            }
        }

        public override bool IsFriendly
        {
            get
            {
                return false;
            }
        }
    }
}
