using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteSlimeXmas : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Holiday Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 52;
            npc.friendly = true;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            Main.npcCatchable[mod.NPCType("CuteSlimeXmas")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeXmasNew");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.xMas == true)
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0.025f * 0.5f;
            }
            else
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0f;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
            if (Main.rand.Next(5) < 1) // a 2 in 7 chance
                Item.NewItem(npc.getRect(), ItemID.GiantBow);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }
    }
}
