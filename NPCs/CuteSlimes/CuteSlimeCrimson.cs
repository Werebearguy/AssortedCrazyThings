using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeCrimson : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Crimson Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeCrimsonNew").Type;
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Crimson;
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

        public override void MoreSetDefaults()
        {
            NPC.scale = 1.2f;
        }
    }
}
