using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class GiantGrasshopperGold : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Golden Grasshopper");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Derpling];
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 44;
            NPC.damage = 1;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 41;
            AIType = NPCID.Derpling;
            AnimationType = NPCID.Derpling;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.001f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.GoldGrasshopper, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(232, 234), hitDirection, -1f);
                }
            }
            else
            {
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_01").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_02").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_02").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("GiantGoldenGrasshopperGore_03").Type, 1f);
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(232, 234), 2 * hitDirection, -2f);
                }
            }
        }

        public override void PostAI()
        {
            //using Microsoft.Xna.Framework;
            //change the npc. to projectile. if you port this to pets
            Color color = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
            if (color.R > 20 || color.B > 20 || color.G > 20)
            {
                int num = color.R;
                if (color.G > num)
                {
                    num = color.G;
                }
                if (color.B > num)
                {
                    num = color.B;
                }
                num /= 30;
                if (Main.rand.Next(300) < num)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 43, 0f, 0f, 254, new Color(255, 255, 0), 0.5f);
                    dust.velocity *= 0f;
                }
            }
        }
    }
}
