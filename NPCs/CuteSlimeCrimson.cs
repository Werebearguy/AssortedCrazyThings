using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteSlimeCrimson : ModNPC
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
            //npc.friendly = true;
            npc.chaseable = false;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
            npc.alpha = 75;
            Main.npcCatchable[mod.NPCType("CuteSlimeCrimson")] = true;
            npc.catchItem = (short)mod.ItemType("CuteSlimeCrimsonNew");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return CuteSlimeBlack.CuteSlimeSpawnChance(spawnInfo, Main.hardMode? (!Main.bloodMoon? SpawnCondition.Crimson.Chance * 0.05f : SpawnCondition.Corruption.Chance * 0.05f) : 0f);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel);
        }
    }
}
