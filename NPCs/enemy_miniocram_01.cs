using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System;

namespace Harblesnargits_Mod_01.NPCs
{
	public class enemy_miniocram_01 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawn of Ocram");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Corruptor];
        }

		public override void SetDefaults()
		{
			npc.width = 92;
			npc.height = 66;
			npc.damage = 95;
			npc.defense = 65;
			npc.lifeMax = 8500;
			npc.HitSound = SoundID.NPCHit14;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1; //5
			//aiType = NPCID.Corruptor;
			animationType = NPCID.Corruptor;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return !NPC.downedGolemBoss ? 0f :
			SpawnCondition.OverworldNightMonster.Chance * 0.005f;
		}

        public override void NPCLoot()
		{
			{
				Item.NewItem(npc.getRect(), ItemID.Emerald);
				if(Main.rand.Next(5) < 1) // a 2 in 7 chance
				Item.NewItem(npc.getRect(), mod.ItemType("pet_Ocram_01"));
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			{
			if (npc.life <= 0)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_miniocram_03"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_miniocram_02"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/gore_miniocram_01"), 1f);
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Main.NewText("pos :" + npc.position);
            Vector2 position4 = npc.position;
            position4.X = position4.X + Main.npcTexture[npc.type].Width * 0.5f; //shadowdodgecount plus
            position4.Y = position4.Y + npc.gfxOffY; //gfxoff
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
            //Main.NewText("drawcolor :" + drawColor);
            Color color = npc.GetAlpha(drawColor) * (0.5f);
            //Main.NewText("color :" + color);
            Vector2 drawPos = position4 - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
            //Main.NewText("drawpos :" + drawPos);
            spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
            position4.X = position4.X - Main.npcTexture[npc.type].Width; //shadowdodgecount plus
            drawPos = position4 - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
            spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
            //}
            return true;
        }

        //Adapted from Vanilla, NPC type 94 Corruptor, AI type 5
        public override void AI()
		{
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
			{
				npc.TargetClosest();
			}
			float num = 4.2f;
            float num2 = 0.022f;
			Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num4 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
			float num5 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
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
				num4 = npc.velocity.X;
				num5 = npc.velocity.Y;
			}
			else
			{
				num6 = num / num6;
				num4 *= num6;
				num5 *= num6;
			}
			if (num7 > 100f)
			{
				npc.ai[0] += 1f;
				if (npc.ai[0] > 0f)
				{
					npc.velocity.Y += 0.023f;
				}
				else
				{
					npc.velocity.Y -= 0.023f;
				}
				if (npc.ai[0] < -100f || npc.ai[0] > 100f)
				{
					npc.velocity.X += 0.023f;
				}
				else
				{
					npc.velocity.X -= 0.023f;
				}
				if (npc.ai[0] > 200f)
				{
                    npc.ai[0] = -200f;
				}
			}
			if (num7 < 150f)
			{
				npc.velocity.X += num4 * 0.007f;
				npc.velocity.Y += num5 * 0.007f;
			}
			if (Main.player[npc.target].dead)
			{
				num4 = (float)npc.direction * num / 2f;
				num5 = (0f - num) / 2f;
			}
            if (npc.velocity.X < num4)
            {
                npc.velocity.X += num2;
            }
            else if (npc.velocity.X > num4)
            {
                npc.velocity.X -= num2;
            }
            if (npc.velocity.Y < num5)
            {
                npc.velocity.Y += num2;
            }
            else if (npc.velocity.Y > num5)
            {
                npc.velocity.Y -= num2;
            }
            npc.rotation = (float)Math.Atan2((double)num5, (double)num4) - 1.57f;

            //doesn't seem to do anything
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
			if (npc.wet)
			{
				if (npc.velocity.Y > 0f)
				{
					npc.velocity.Y *= 0.95f;
				}
				npc.velocity.Y -= 0.3f;
				if (npc.velocity.Y < -2f)
				{
					npc.velocity.Y = -2f;
				}
			}
			if (Main.netMode != 1 && !Main.player[npc.target].dead)
			{
                //localAI[0] is the timer for the projectile shoot
				if (npc.justHit)
				{
                    //makes it so it doesn't shoot projectiles when it's hit
					//npc.localAI[0] = 0f;
				}
                npc.localAI[0] += 1f;
                float shootDelay = 180f;
				if (npc.localAI[0] == shootDelay)
				{
                    int projectileDamage = 21;
                    int projectileType = 44; //Demon Scythe
                    int projectileTravelTime = 180;
                    float num224 = 0.2f;
                    Vector2 vector27 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    float num225 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-50, 51);
                    float num226 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-50, 51);
                    float num227 = (float)Math.Sqrt((double)(num225 * num225 + num226 * num226));
                    num227 = num224 / num227;
                    num225 *= num227;
                    num226 *= num227;
                    int leftScythe = Projectile.NewProjectile(vector27.X - npc.width * 0.5f, vector27.Y, num225, num226, projectileType, projectileDamage, 0f, npc.target);
                    Main.projectile[leftScythe].tileCollide = false;
                    Main.projectile[leftScythe].timeLeft = projectileTravelTime;

                    num225 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector27.X + (float)Main.rand.Next(-50, 51);
                    num226 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector27.Y + (float)Main.rand.Next(-50, 51);
                    num227 = (float)Math.Sqrt((double)(num225 * num225 + num226 * num226));
                    num227 = num224 / num227;
                    num225 *= num227;
                    num226 *= num227;
                    int rightScythe = Projectile.NewProjectile(vector27.X + npc.width * 0.5f, vector27.Y, num225, num226, projectileType, projectileDamage, 0f, npc.target);
                    Main.projectile[rightScythe].tileCollide = false;
                    Main.projectile[rightScythe].timeLeft = projectileTravelTime;

                    //NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), 112);
                    //PROJECTILE IS ACTUALLY AN NPC AHAHAHAHAHAHAHAHHAAH
                    //https://terraria.gamepedia.com/Vile_Spit

                    npc.localAI[0] = 0f;
				}
			}
            //
            //  Main.dayTime || Main.player[npc.target].dead
            //  vvvvvvvvvvvv
            if (Main.player[npc.target].dead)
			{
				npc.velocity.Y -= num2 * 2f;
				if (npc.timeLeft > 10)
				{
                    npc.timeLeft = 10;
				}
			}
			if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
			{
                npc.netUpdate = true;
			}
		}
    }
}
