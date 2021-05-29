using AssortedCrazyThings.Base;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeIce : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Ice Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return Mod.Find<ModItem>("CuteSlimeIceNew").Type;
            }
        }

        public override SpawnConditionType SpawnCondition
        {
            get
            {
                return SpawnConditionType.Tundra;
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
            NPC.scale = 0.9f;
        }
    }
}
