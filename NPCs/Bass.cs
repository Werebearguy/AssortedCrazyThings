using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Bass : ModNPC
    {
        public float scareRange = 200f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bass");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 32;
            NPC.damage = 0;
            NPC.defense = 0;
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
            if (Main.raining == true)
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.8f;
            }
            else
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.05f;
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Bass);
        }

        public override void AI()
        {
            AssAI.ModifiedGoldfishAI(NPC, 200f);
        }
    }
}
