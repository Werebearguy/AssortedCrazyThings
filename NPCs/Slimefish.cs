using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Slimefish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slimefish");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 36;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 16;
            AIType = NPCID.Goldfish;
            AnimationType = NPCID.Goldfish;
            NPC.noGravity = true;
            Main.npcCatchable[Mod.Find<ModNPC>("Slimefish").Type] = true;
            NPC.catchItem = ItemID.Slimefish;
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
            Item.NewItem(NPC.getRect(), ItemID.Gel);
        }
    }
}
