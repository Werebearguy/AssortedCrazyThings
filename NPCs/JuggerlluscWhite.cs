using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class JuggerlluscWhite : ModNPC
    {
        public static string name = "Juggerllusc";
        public static string message = "A large snail is approaching!";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SeaSnail];
        }

        public override void SetDefaults()
        {
            npc.width = 112;
            npc.height = 67;
            npc.damage = 40;
            npc.defense = 20;
            npc.lifeMax = 900;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 640f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 3;
            aiType = NPCID.SeaSnail;
            animationType = NPCID.SeaSnail;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Slow, 240, true);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.downedBoss1 ? SpawnCondition.Ocean.Chance * 0.005f : 0f;
        }

        // Whether or not to run the code for checking whether this NPC will remain active.
        //Return false to stop this NPC from being despawned and to stop this NPC from
        //counting towards the limit for how many NPCs can exist near a player. Returns true by default.
        //public override bool CheckActive()
        //{
        //    return false;
        //}
        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.PurpleMucos);
            if (Main.rand.NextBool(20))
            {
                Item.NewItem(npc.getRect(), ItemID.Flipper);
            }
            if (Main.rand.NextBool(50))
            {
                Item.NewItem(npc.getRect(), ItemID.DivingHelmet);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_megasnail_01"), 1f);
            }
        }
    }
}
