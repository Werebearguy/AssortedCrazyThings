using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
    public class enemy_slime_07 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunky");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 26;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Corruption.Chance * 0.1f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.RottenChunk);
            if (Main.rand.NextBool(100))
                Item.NewItem(npc.getRect(), mod.ItemType("combine_Eye_01"));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.life <= 0 && Main.rand.NextBool(20))
                {
                    NPC.NewNPC((int)npc.position.X, (int)npc.position.Y - 16, mod.NPCType("enemy_eye_ex01"));
                }
            }
        }
    }
}
