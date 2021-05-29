using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.FoldfishBoss
{
    public class FoldfishBaby : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foldfish Baby");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 20;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = -1; //custom
            AIType = NPCID.Goldfish;
            AnimationType = NPCID.Goldfish;
            NPC.noGravity = true;
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void AI()
        {
            //modified goldfish AI
            NPC.direction = NPC.whoAmI % 2;
            NPC.spriteDirection = NPC.direction;
            if (NPC.velocity.Y == 0f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.velocity.Y = Main.rand.Next(-50, -20) * 0.1f;
                    NPC.velocity.X = Main.rand.Next(-20, 20) * 0.1f;
                    NPC.netUpdate = true;
                }
            }
            NPC.velocity.Y = NPC.velocity.Y + 0.3f;
            if (NPC.velocity.Y > 10f)
            {
                NPC.velocity.Y = 10f;
            }
            NPC.ai[0] = 1f;

            NPC.rotation = NPC.velocity.Y * NPC.direction * 0.1f;
            if (NPC.rotation < -0.2f)
            {
                NPC.rotation = -0.2f;
            }
            if (NPC.rotation > 0.2f)
            {
                NPC.rotation = 0.2f;
            }
        }
    }
}
