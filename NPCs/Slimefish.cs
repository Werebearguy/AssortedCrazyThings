using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
    public class Slimefish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slimefish");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 36;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 16;
            aiType = NPCID.Goldfish;
            animationType = NPCID.Goldfish;
            npc.noGravity = true;
            Main.npcCatchable[mod.NPCType("Slimefish")] = true;
            npc.catchItem = ItemID.Slimefish;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.raining == true)
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.8f;
            }
            else
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.05f;
            }
        }

        public override void NPCLoot()
        {
            {
                Item.NewItem(npc.getRect(), ItemID.Gel);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            {
                if (npc.life <= 0)
                {
                }
            }
        }
    }
}
