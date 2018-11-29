using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
    public class enemy_fish_02 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloudfish");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 36;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 16;
            aiType = NPCID.Goldfish;
            animationType = NPCID.Goldfish;
            npc.noGravity = true;
            Main.npcCatchable[mod.NPCType("enemy_fish_02")] = true;
            npc.catchItem = ItemID.Cloudfish;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Main.NewText(spawnInfo.player.ZoneSkyHeight);
            if (spawnInfo.player.ZoneSkyHeight)
            {
                return SpawnCondition.Sky.Chance * 100f; //0.05f before, 100f now because water check
            }
            else
            {
                return SpawnCondition.TownWaterCritter.Chance * 0.001f;
            }
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            if (Main.tile[tileX, tileY].liquid == 0)
            {
                return 0;
            }
            else if (
               !WorldGen.SolidTile(tileX, tileY) &&
               !WorldGen.SolidTile(tileX, tileY + 1) &&
               !WorldGen.SolidTile(tileX, tileY + 2))
            {
                //actually spawn
                return base.SpawnNPC(tileX, tileY);
            }
            return 0;
        }

        public override void NPCLoot()
        {
            {
                Item.NewItem(npc.getRect(), ItemID.Cloud, 10 + Main.rand.Next(20));
                if (Main.rand.NextBool(10))
                    Item.NewItem(npc.getRect(), ItemID.RainCloud, 10 + Main.rand.Next(20));
                if (Main.rand.NextBool(15))
                    Item.NewItem(npc.getRect(), ItemID.SnowCloudBlock, 10 + Main.rand.Next(20));
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            {
                if (npc.life <= 0)
                {
                }
            }
        }
    }
}
