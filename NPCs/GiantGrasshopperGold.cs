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
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Derpling];
        }

        public override void SetDefaults()
        {
            npc.width = 64;
            npc.height = 44;
            npc.damage = 1;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath4;
            npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 41;
            aiType = NPCID.Derpling;
            animationType = NPCID.Derpling;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.001f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.GoldGrasshopper, 1);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, Main.rand.Next(232, 234), hitDirection, -1f);
                }
            }
            else
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_01"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_02"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_02"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_03"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_03"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_03"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GiantGoldenGrasshopperGore_03"), 1f);
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, Main.rand.Next(232, 234), 2 * hitDirection, -2f);
                }
            }
        }

        public override void PostAI()
        {
            //using Microsoft.Xna.Framework;
            //change the npc. to projectile. if you port this to pets
            Color color = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
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
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 43, 0f, 0f, 254, new Color(255, 255, 0), 0.5f);
                    dust.velocity *= 0f;
                }
            }
        }
    }
}
