using AssortedCrazyThings.Base;
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
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingSnake];
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 52;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 20;
            NPC.friendly = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 14;
            AIType = NPCID.FlyingSnake;
            AnimationType = NPCID.FlyingSnake;
            Main.npcCatchable[ModContent.NPCType<CuteGastropod>()] = true;
            NPC.catchItem = (short)Mod.Find<ModItem>("CuteGastropod").Type;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SlimePets.CuteSlimeSpawnChance(spawnInfo, SpawnConditionType.None, customFactor: !NPC.AnyNPCs(ModContent.NPCType<CuteGastropod>()) ? SpawnCondition.OverworldHallow.Chance * 0.045f : 0f);
        }
    }
}
