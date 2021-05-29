using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class YoungHarpy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Harpy");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingSnake];
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 44;
            NPC.damage = 0;
            NPC.defense = 1;
            NPC.lifeMax = 5;
            NPC.friendly = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 14;
            AIType = NPCID.FlyingSnake;
            AnimationType = NPCID.FlyingSnake;
            Main.npcCatchable[Mod.Find<ModNPC>("YoungHarpy").Type] = true;
            NPC.catchItem = (short)Mod.Find<ModItem>("YoungHarpy").Type;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Sky.Chance * 0.05f;
        }
    }
}
