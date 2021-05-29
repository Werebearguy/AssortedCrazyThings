using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class UnfortunateDelver : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unfortunate Delver");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BartenderUnconscious];
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 46;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            AIType = NPCID.BartenderUnconscious;
            AnimationType = NPCID.BartenderUnconscious;
            NPC.friendly = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.marble) return 0.005f;
            else return 0f;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.StoneBlock, 4 + Main.rand.Next(5));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/UnfortunateDelverGore_01").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/UnfortunateDelverGore_02").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("AssortedCrazyThings/UnfortunateDelverGore_03").Type, 1f);
            }
        }
    }
}
