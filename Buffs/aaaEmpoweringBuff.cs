using AssortedCrazyThings.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class aaaEmpoweringBuff : ModBuff
    {
        public override void SetDefaults()
        {
            //purely on the NPC side, so no name or anything required
            //applied in AssPlayer.PreUpdate()
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<AssGlobalNPC>(mod).shouldSoulDrop = true;
        }
    }
}