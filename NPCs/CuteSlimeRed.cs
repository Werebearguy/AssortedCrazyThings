using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteSlimeRed : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 52;
            npc.scale = 1.025f;
            npc.friendly = true;
            npc.damage = 12;
            npc.defense = 4;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 1f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 125;
            npc.color = new Color(255, 30, 0, 100);
            Main.npcCatchable[mod.NPCType("CuteSlimeRed")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeRed");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.025f * 0.5f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }
    }
}
