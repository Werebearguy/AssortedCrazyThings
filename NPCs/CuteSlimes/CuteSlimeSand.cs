using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeSand : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Sand Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeSandNew").Type;
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
                return SpawnConditionType.Desert;
            }
        }

        public override void MoreSetDefaults()
        {
            NPC.alpha = 45;
        }
    }
}
