using AssortedCrazyThings.Items.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Chunky : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 26;
            NPC.damage = 7;
            NPC.defense = 2;
            NPC.lifeMax = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 20f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 1;
            AIType = NPCID.ToxicSludge;
            AnimationType = NPCID.ToxicSludge;
            NPC.catchItem = (short)ModContent.ItemType<ChunkyItem>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode) return SpawnCondition.Corruption.Chance * 0.05f;
            return SpawnCondition.Corruption.Chance * 0.2f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.RottenChunk);
            if (Main.rand.NextBool(10))
            {
                int i = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y - 16, Mod.Find<ModNPC>("ChunkysEye").Type);
                if (Main.netMode == NetmodeID.Server && i < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
                }
            }
        }
    }
}
