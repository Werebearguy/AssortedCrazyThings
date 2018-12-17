using Microsoft.Xna.Framework;
using System;
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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 18;
            npc.damage = 0;
            npc.defense = 20;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = -1; //custom
            aiType = NPCID.Goldfish;
            animationType = NPCID.Goldfish;
            npc.noGravity = true;
            npc.buffImmune[BuffID.Confused] = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

		public override void AI()
		{
            //modified foldfish AI
            npc.direction = npc.whoAmI % 2;
            npc.spriteDirection = npc.direction;
			if (npc.velocity.Y == 0f)
			{
				if (Main.netMode != 1)
				{
					npc.velocity.Y = (float)Main.rand.Next(-50, -20) * 0.1f;
					npc.velocity.X = (float)Main.rand.Next(-20, 20) * 0.1f;
					npc.netUpdate = true;
				}
			}
			npc.velocity.Y = npc.velocity.Y + 0.3f;
			if (npc.velocity.Y > 10f)
			{
				npc.velocity.Y = 10f;
			}
			npc.ai[0] = 1f;

			npc.rotation = npc.velocity.Y * (float)npc.direction * 0.1f;
			if ((double)npc.rotation < -0.2)
			{
				npc.rotation = -0.2f;
			}
			if ((double)npc.rotation > 0.2)
			{
				npc.rotation = 0.2f;
			}
		}
    }
}
