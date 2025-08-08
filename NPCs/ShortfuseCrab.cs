using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.FriendlyNPCs)]
	public class ShortfuseCrab : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 7;

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;

			NPCID.Sets.CountsAsCritter[NPC.type] = true; //Guide To Critter Companionship
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 36;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.aiStyle = 3;
			AIType = NPCID.Crab;
			NPC.catchItem = ModContent.ItemType<ShortfuseCrabItem>();
		}

		public override void FindFrame(int frameHeight)
		{
			//0 idle, 1 airborne, rest walk cycle
			if (NPC.velocity.Y == 0f)
			{
				NPC.spriteDirection = NPC.direction;
			}
			else if (Math.Abs(NPC.velocity.Y) > 0.5f)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = 1;
			}

			if (NPC.velocity.X == 0f)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = 0;
			}
			else
			{
				int firstWalkFrame = 2 * frameHeight;
				if (NPC.frame.Y < firstWalkFrame)
				{
					NPC.frame.Y = firstWalkFrame;
				}

				NPC.frameCounter += 1;
				if (NPC.frameCounter >= 6)
				{
					NPC.frameCounter = 0;
					NPC.frame.Y += frameHeight;
					if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
					{
						NPC.frame.Y = firstWalkFrame;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Ocean.Chance * 0.012f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++) //40
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.8f);
					dust.velocity *= 2f; //3f
					if (Main.rand.NextBool(2))
					{
						dust.scale = 0.5f;
						dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int i = 0; i < 17; i++) //70
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 3f);
					dust.noGravity = true;
					dust.velocity *= 4f; //5f
					dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, default(Color), 2f);
					dust.velocity *= 2f;
				}
				for (int i = 0; i < 2; i++) //3
				{
					float scaleFactor10 = 0.33f;
					if (i == 1)
					{
						scaleFactor10 = 0.66f;
					}
					if (i == 2)
					{
						scaleFactor10 = 1f;
					}
					var entitySource = NPC.GetSource_FromAI();
					Gore gore = Main.gore[Gore.NewGore(entitySource, NPC.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += 1f;
					gore.velocity.Y += 1f;
					gore = Main.gore[Gore.NewGore(entitySource, NPC.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += -1f;
					gore.velocity.Y += 1f;
					gore = Main.gore[Gore.NewGore(entitySource, NPC.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += 1f;
					gore.velocity.Y += -1f;
					gore = Main.gore[Gore.NewGore(entitySource, NPC.Center - new Vector2(24f), default(Vector2), Main.rand.Next(61, 64), 1f)];
					gore.velocity *= scaleFactor10;
					gore.velocity.X += -1f;
					gore.velocity.Y += -1f;
				}
			}
		}
	}
}
