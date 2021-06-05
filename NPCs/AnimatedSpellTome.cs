using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class AnimatedSpellTome : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Spell Tome");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 16;
            NPC.damage = 13;
            NPC.defense = 2;
            NPC.lifeMax = 16;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.8f;
            NPC.aiStyle = 14;
            NPC.noGravity = true;
            AIType = NPCID.GiantBat;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.LoopAnimation(frameHeight, 8);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.DungeonNormal.Chance * 0.005f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("Master wizards need not look for a specific tome; it will find them when needed.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SpellTome));
        }

        //golden dust particles on hit and passively spawning sparkles in the next two methods
        //make sure to do "using Microsoft.Xna.Framework;"
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
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.Next(232, 234), 2 * hitDirection, -2f);
                }

                for (int u = 0; u < 6; u++)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                    Gore gore = Gore.NewGoreDirect(pos, NPC.velocity * 0.5f, Mod.Find<ModGore>("PaperScrapGore").Type, 1f);
                    gore.velocity += new Vector2(Main.rand.NextFloat(3) - 1f, Main.rand.NextFloat(MathHelper.TwoPi) - 0.3f);
                }

                for (int i = 0; i < 3; i++)
                {
                    Gore gore = Gore.NewGoreDirect(NPC.position, NPC.velocity.SafeNormalize(Vector2.UnitY) * 3f, Mod.Find<ModGore>("PaperGore").Type, 1f + Main.rand.NextFloatDirection() * 0.2f);
                    gore.velocity += new Vector2(Main.rand.NextFloat(2) - 1f, Main.rand.NextFloat(MathHelper.TwoPi) - 0.3f);
                    gore.velocity *= 4f;
                }
            }
        }

        public override void PostAI()
        {
            NPC.rotation = NPC.velocity.X * 0.06f;

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
