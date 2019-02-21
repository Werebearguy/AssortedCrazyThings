using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class CuteGastropod : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cute Gastropod");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingSnake];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 52;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 14;
            aiType = NPCID.FlyingSnake;
            animationType = NPCID.FlyingSnake;
            Main.npcCatchable[mod.NPCType<CuteGastropod>()] = true;
            npc.catchItem = (short)mod.ItemType("CuteGastropod");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.AnyNPCs(mod.NPCType<CuteGastropod>())) return SpawnCondition.OverworldHallow.Chance * 0.05f;
            else return 0f;
        }
    }
}
