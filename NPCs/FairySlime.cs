using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class FairySlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fairy Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 30;
            NPC.damage = 7;
            NPC.defense = 2;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 25f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 1;
            AIType = NPCID.ToxicSludge;
            AnimationType = NPCID.ToxicSludge;
            NPC.alpha = 175;
            NPC.color = new Color(213, 196, 197, 100);
            NPC.catchItem = (short)ModContent.ItemType<FairySlimeItem>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldHallow.Chance * 0.015f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                new FlavorTextBestiaryInfoElement("Not wanting to lose to its flying neighbors, it has grown wing-like extentions. It can't fly, but it's happy regardless of this fact.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel));
        }
    }
}
