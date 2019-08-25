using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    public class CuteSlimeRainbow : CuteSlimeBaseNPC
    {
        public override string IngameName
        {
            get
            {
                return "Cute Rainbow Slime";
            }
        }

        public override int CatchItem
        {
            get
            {
                return mod.ItemType("CuteSlimeRainbowNew");
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
            npc.scale = 1.2f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Main.DiscoColor;
            lightColor *= (255f - npc.alpha) / 255f;
            return lightColor;
        }
    }
}
