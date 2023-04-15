using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class SpawnOfOcram : AssNPC
	{
		public override void SetStaticDefaults()
		{
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
			if (NPC.downedGolemBoss && !NPC.AnyNPCs(NPC.type))
			{
				return SpawnCondition.OverworldNightMonster.Chance * 0.005f;
			}
			return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Emerald));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BabyOcramItem>(), chanceDenominator: 5));
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				var entitySource = NPC.GetSource_Death();
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("SpawnOfOcramGore_2").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("SpawnOfOcramGore_1").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("SpawnOfOcramGore_0").Type, 1f);
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
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[npc.type].Value, drawPos, new npc.frame, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);

            position4.X = position4.X - Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width; //shadowdodgecount minus

            drawPos = position4 - screenPos + drawOrigin + new Vector2(0f, npc.gfxOffY);
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[npc.type].Value, drawPos, new npc.frame, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
            */

			Texture2D texture = TextureAssets.Npc[NPC.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
			//the higher the k, the older the position
			//Length is implicitely set in TrailCacheLength up there
			//start from half the length so the origninal sprite isnt super blurred
			for (int k = (NPC.oldPos.Length / 3); k < NPC.oldPos.Length; k++)
			{
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (2f * NPC.oldPos.Length));
				spriteBatch.Draw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public ref float Timer => ref NPC.ai[0];

		public ref float AttackTimer => ref NPC.localAI[0];


		//Adapted from Vanilla, NPC type 94 Corruptor, AI type 5
		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest();
			}
			Player player = Main.player[NPC.target];

			float num2 = 0.022f;
			if (player.dead)
			{
				NPC.velocity.Y -= num2 * 2f;
				if (NPC.timeLeft > 10)
				{
					NPC.timeLeft = 10;
					return;
				}
			}

			float num = 4.2f;
			Vector2 vector = NPC.Center;
			float num4 = player.Center.X;
			float num5 = player.Center.Y;
			num4 = (int)(num4 / 8f) * 8;
			num5 = (int)(num5 / 8f) * 8;
			vector.X = (int)(vector.X / 8f) * 8;
			vector.Y = (int)(vector.Y / 8f) * 8;
			num4 -= vector.X;
			num5 -= vector.Y;
			float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
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
				Timer += 1f;
				if (Timer > 0f)
				{
					NPC.velocity.Y += 0.023f;
				}
				else
				{
					NPC.velocity.Y -= 0.023f;
				}
				if (Timer < -100f || Timer > 100f)
				{
					NPC.velocity.X += 0.023f;
				}
				else
				{
					NPC.velocity.X -= 0.023f;
				}
				if (Timer > 200f)
				{
					Timer = -200f;
				}
			}
			if (num7 < 150f)
			{
				NPC.velocity.X += num4 * 0.007f;
				NPC.velocity.Y += num5 * 0.007f;
			}
			//if (player.dead)
			//{
			//    num4 = NPC.direction * num / 2f;
			//    num5 = (0f - num) / 2f;
			//}
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
			NPC.rotation = (float)Math.Atan2(num5, num4) - MathHelper.PiOver2;

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
			if (Main.netMode != NetmodeID.MultiplayerClient && !player.dead)
			{
				if (NPC.justHit)
				{
					//makes it so it doesn't shoot projectiles when it's hit
					//AttackTimer = 0f;
				}
				AttackTimer += 1f;
				float shootDelay = 180f;
				if (AttackTimer >= shootDelay)
				{
					AttackTimer = 0f;

					int projectileDamage = 21;
					int projectileType = ProjectileID.DemonSickle;
					int projectileTravelTime = 70;
					Vector2 center = NPC.Center;

					Vector2 randomCenter = center + new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
					Vector2 toPlayer = player.DirectionFrom(randomCenter);
					toPlayer *= 4;
					int leftSickle = Projectile.NewProjectile(NPC.GetSource_FromAI(), center.X - NPC.width * 0.5f, center.Y, toPlayer.X, toPlayer.Y, projectileType, projectileDamage, 0f, Main.myPlayer);
					Main.projectile[leftSickle].tileCollide = false;
					Main.projectile[leftSickle].timeLeft = projectileTravelTime;

					randomCenter = center + new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
					toPlayer = player.DirectionFrom(randomCenter);
					toPlayer *= 4;
					int rightSickle = Projectile.NewProjectile(NPC.GetSource_FromAI(), center.X + NPC.width * 0.5f, center.Y, toPlayer.X, toPlayer.Y, projectileType, projectileDamage, 0f, Main.myPlayer);
					Main.projectile[rightSickle].tileCollide = false;
					Main.projectile[rightSickle].timeLeft = projectileTravelTime;

					//NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), 112);
					//PROJECTILE IS ACTUALLY AN NPC AHAHAHAHAHAHAHAHHAAH
					//https://terraria.gamepedia.com/Vile_Spit
				}
			}
			//
			//  Main.dayTime || Main.player[npc.target].dead
			//  vvvvvvvvvvvv
			if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
			{
				NPC.netUpdate = true;
			}
		}
	}
}
