using AssortedCrazyThings.NPCs;
using Terraria;

namespace AssortedCrazyThings.Buffs
{
    [Content(ContentType.Bosses)]
    public class SoulBuff : AssBuff
    {
        public override string Texture => "AssortedCrazyThings/Empty";

        public override void SetDefaults()
        {
            //purely on the NPC side, so no name or texture required
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<HarvesterGlobalNPC>().shouldSoulDrop = true;
        }
    }
}