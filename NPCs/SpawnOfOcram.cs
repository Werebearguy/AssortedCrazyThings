using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class SpawnOfOcram : ModNPC
    {
        public static string name = "Spawn of Ocram";
        public static string message = "Spawn of Ocram has appeared!";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Corruptor];
            //same as chaos elemental, tho for npcs you still have to manually draw it (PreDraw())
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 92;
            NPC.height = 66;
            NPC.damage = 95;
            NPC.defense = 40;
            NPC.lifeMax = 4200;
            NPC.HitSound = SoundID.NPCHit14;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1; //5
                              //AIType = NPCID.Corruptor;
            AnimationType = NPCID.Corruptor;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.buffImmune[BuffID.Confused] = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedGolemBoss && !NPC.AnyNPCs(ModContent.NPCType<SpawnOfOcram>()))
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.005f;
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("An ancient being's offspring, once thought to have been lost to time.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Emerald));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BabyOcramItem>(), chanceDenominator: 5));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            {
                if (NPC.life <= 0)
                {
                    Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("SpawnOfOcramGore_2").Type, 1f);
                    Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("SpawnOfOcramGore_1").Type, 1f);
                    Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("SpawnOfOcramGore_0").Type, 1f);
                }
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return new Color
            {
                R = Utils.Clamp<byte>(drawColor.R, 100, 255),
                G = Utils.Clamp<byte>(drawColor.G, 100, 255),
                B = Utils.Clamp<byte>(drawColor.B, 100, 255),
                A = 255
            };
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*Replica of titanium armor effect (Shadow dodge)
            Color color = npc.GetAlpha(drawColor) * (0.5f);
            Vector2 position4 = npc.position;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f, npc.height * 0.5f);
            position4.Y = position4.Y + npc.gfxOffY; //gfxoff

            position4.X = position4.X + Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f; //shadowdodgecount plus

            Vector2 drawPos = position4 - screenPos + drawOrigin + new Vector2(0f, npc.gfxOffY);
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[npc.type].Value, drawPos, new npc.frame, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);

            position4.X = position4.X - Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width; //shadowdodgecount minus

            drawPos = position4 - screenPos + drawOrigin + new Vector2(0f, npc.gfxOffY);
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[npc.type].Value, drawPos, new npc.frame, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
            */

            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
            //the higher the k, the older the position
            //Length is implicitely set in TrailCacheLength up there
            //start from half the length so the origninal sprite isnt super blurred
            for (int k = (NPC.oldPos.Length / 3); k < NPC.oldPos.Length; k++)
            {
                Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (2f * NPC.oldPos.Length));
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        //Adapted from Vanilla, NPC type 94 Corruptor, AI type 5
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest();
            }
            float num = 4.2f;
            float num2 = 0.022f;
            Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num4 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float num5 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            num4 = (float)((int)(num4 / 8f) * 8);
            num5 = (float)((int)(num5 / 8f) * 8);
            vector.X = (float)((int)(vector.X / 8f) * 8);
            vector.Y = (float)((int)(vector.Y / 8f) * 8);
            num4 -= vector.X;
            num5 -= vector.Y;
            float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
            float num7 = num6;
            if (num6 == 0f)
            {
                num4 = NPC.velocity.X;
                num5 = NPC.velocity.Y;
            }
            else
            {
                num6 = num / num6;
                num4 *= num6;
                num5 *= num6;
            }
            if (num7 > 100f)
            {
                NPC.ai[0] += 1f;
                if (NPC.ai[0] > 0f)
                {
                    NPC.velocity.Y += 0.023f;
                }
                else
                {
                    NPC.velocity.Y -= 0.023f;
                }
                if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
                {
                    NPC.velocity.X += 0.023f;
                }
                else
                {
                    NPC.velocity.X -= 0.023f;
                }
                if (NPC.ai[0] > 200f)
                {
                    NPC.ai[0] = -200f;
                }
            }
            if (num7 < 150f)
            {
                NPC.velocity.X += num4 * 0.007f;
                NPC.velocity.Y += num5 * 0.007f;
            }
            if (Main.player[NPC.target].dead)
            {
                num4 = (float)NPC.direction * num / 2f;
                num5 = (0f - num) / 2f;
            }
            if (NPC.velocity.X < num4)
            {
                NPC.velocity.X += num2;
            }
            else if (NPC.velocity.X > num4)
            {
                NPC.velocity.X -= num2;
            }
            if (NPC.velocity.Y < num5)
            {
                NPC.velocity.Y += num2;
            }
            else if (NPC.velocity.Y > num5)
            {
                NPC.velocity.Y -= num2;
            }
            NPC.rotation = (float)Math.Atan2((double)num5, (double)num4) - 1.57f;

            //doesn't seem to do anything because npc.notilecollide is set to false
            //float num12 = 0.7f;
            //if (npc.collideX)
            //{
            //	npc.netUpdate = true;
            //	npc.velocity.X = npc.oldVelocity.X * (0f - num12);
            //	if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
            //	{
            //		npc.velocity.X = 2f;
            //	}
            //	if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
            //	{
            //		npc.velocity.X = -2f;
            //	}
            //}
            //if (npc.collideY)
            //{
            //  npc.netUpdate = true;
            //	npc.velocity.Y = npc.oldVelocity.Y * (0f - num12);
            //	if (npc.velocity.Y > 0f && (double)npc.velocity.Y < 1.5)
            //	{
            //		npc.velocity.Y = 2f;
            //	}
            //	if (npc.velocity.Y < 0f && (double)npc.velocity.Y > -1.5)
            //	{
            //		npc.velocity.Y = -2f;
            //	}
            //}
            if (NPC.wet)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
                NPC.velocity.Y -= 0.3f;
                if (NPC.velocity.Y < -2f)
                {
                    NPC.velocity.Y = -2f;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && !Main.player[NPC.target].dead)
            {
                //localAI[0] is the timer for the projectile shoot
                if (NPC.justHit)
                {
                    //makes it so it doesn't shoot projectiles when it's hit
                    //npc.localAI[0] = 0f;
                }
                NPC.localAI[0] += 1f;
                float shootDelay = 180f;
                if (NPC.localAI[0] == shootDelay)
                {
                    int projectileDamage = 21;
                    int projectileType = 44; //Demon Scythe
                    int projectileTravelTime = 70;
                    float num224 = 0.2f;
                    Vector2 vector27 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    float num225 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-50, 51);
                    float num226 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-50, 51);
                    float num227 = (float)Math.Sqrt((double)(num225 * num225 + num226 * num226));
                    num227 = num224 / num227;
                    num225 *= num227;
                    num226 *= num227;
                    num225 *= 20;
                    num226 *= 20;
                    int leftScythe = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), vector27.X - NPC.width * 0.5f, vector27.Y, num225, num226, projectileType, projectileDamage, 0f, Main.myPlayer);
                    Main.projectile[leftScythe].tileCollide = false;
                    Main.projectile[leftScythe].timeLeft = projectileTravelTime;

                    num225 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-50, 51);
                    num226 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-50, 51);
                    num227 = (float)Math.Sqrt((double)(num225 * num225 + num226 * num226));
                    num227 = num224 / num227;
                    num225 *= num227;
                    num226 *= num227;
                    num225 *= 20;
                    num226 *= 20;
                    int rightScythe = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), vector27.X + NPC.width * 0.5f, vector27.Y, num225, num226, projectileType, projectileDamage, 0f, Main.myPlayer);
                    Main.projectile[rightScythe].tileCollide = false;
                    Main.projectile[rightScythe].timeLeft = projectileTravelTime;

                    //NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), 112);
                    //PROJECTILE IS ACTUALLY AN NPC AHAHAHAHAHAHAHAHHAAH
                    //https://terraria.gamepedia.com/Vile_Spit

                    NPC.localAI[0] = 0f;
                }
            }
            //
            //  Main.dayTime || Main.player[npc.target].dead
            //  vvvvvvvvvvvv
            if (Main.player[NPC.target].dead)
            {
                NPC.velocity.Y -= num2 * 2f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }
            if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
            {
                NPC.netUpdate = true;
            }
        }
    }
}
