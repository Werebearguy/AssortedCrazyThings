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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BartenderUnconscious];
        }

        public override void SetDefaults()
        {
            npc.width = 28;
            npc.height = 46;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 0;
            aiType = NPCID.BartenderUnconscious;
            animationType = NPCID.BartenderUnconscious;
            npc.friendly = true;
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

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.StoneBlock, 4 + Main.rand.Next(5));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UnfortunateDelverGore_01"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UnfortunateDelverGore_02"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UnfortunateDelverGore_03"), 1f);
            }
        }
    }
}
