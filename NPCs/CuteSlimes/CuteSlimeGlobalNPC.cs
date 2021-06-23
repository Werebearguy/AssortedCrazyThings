using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
    [Content(ContentType.CuteSlimes)]
    public class CuteSlimeGlobalNPC : AssGlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CuteSlimePrincessItem>(), chanceDenominator: 10));
            }
            else if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CuteSlimeQueenItem>(), chanceDenominator: 10));
            }
        }
    }
}
