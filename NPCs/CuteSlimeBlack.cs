using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteSlimeBlack : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 52;
            npc.scale = 0.9f;
            npc.friendly = true;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.9f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 75;
            Main.npcCatchable[mod.NPCType("CuteSlimeBlack")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeBlackNew");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ModConf.CuteSlimes) return SpawnCondition.OverworldDaySlime.Chance * 0.025f * 0.5f;
            else return 0f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }
    }
}
