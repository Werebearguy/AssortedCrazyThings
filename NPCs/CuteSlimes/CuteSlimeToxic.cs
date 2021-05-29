using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeToxic : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Toxic Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeToxicNew").Type;
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
                return SpawnConditionType.Underground;
            }
        }
    }
}
