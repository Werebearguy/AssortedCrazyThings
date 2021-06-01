using AssortedCrazyThings.Items.Pets;
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
            Main.npcCatchable[NPC.type] = true;
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
            NPC.catchItem = (short)ModContent.ItemType<YoungHarpyItem>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Sky.Chance * 0.05f;
        }
    }
}
