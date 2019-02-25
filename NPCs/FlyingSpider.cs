using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class FlyingSpider : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Spider");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingSnake];
        }

        public override void SetDefaults()
        {
            npc.width = 76;
            npc.height = 48;
            npc.damage = 45;
            npc.defense = 17;
            npc.lifeMax = 195;
            npc.HitSound = SoundID.NPCHit29;
            npc.DeathSound = SoundID.NPCDeath31;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 14;
            aiType = NPCID.FlyingSnake;
            animationType = NPCID.FlyingSnake;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode == true)
            {
                return SpawnCondition.SpiderCave.Chance * 0.005f;
            }
            else
            {
                return SpawnCondition.SpiderCave.Chance * 0f;
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(1) == 0 && Main.expertMode)
                Item.NewItem(npc.getRect(), ItemID.SpiderFang);
            if (Main.rand.Next(4) < 3)
                Item.NewItem(npc.getRect(), ItemID.SpiderFang, 1 + Main.rand.Next(3)); // 1, 2, or 3
            if (Main.rand.Next(100) < 2)
                Item.NewItem(npc.getRect(), ItemID.PoisonStaff);
        }
    }
}
