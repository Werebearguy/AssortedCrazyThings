using AssortedCrazyThings.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    [Content(ContentType.Boss)]
    public class SoulBuff : AssBuff
    {
        public override string Texture => "AssortedCrazyThings/Empty";

        public override void SetDefaults()
        {
            //purely on the NPC side, so no name or anything required
            //applied in AssPlayer.PreUpdate()
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<AssGlobalNPC>().shouldSoulDrop = true;
        }
    }
}