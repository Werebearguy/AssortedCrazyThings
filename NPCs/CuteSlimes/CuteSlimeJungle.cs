using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
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
                return ModContent.ItemType<CuteSlimeJungleItem>();
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
