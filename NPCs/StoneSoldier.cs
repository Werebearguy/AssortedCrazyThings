using AssortedCrazyThings.Base;
using AssortedCrazyThings.NPCs.DropConditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class StoneSoldier : AssNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Soldier");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ArmedZombie];
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 30;
			NPC.defense = 16;
			NPC.lifeMax = 50;
			NPC.HitSound = SoundID.NPCHit41;
			NPC.DeathSound = SoundID.NPCDeath52;
			NPC.value = 60f;
			NPC.knockBackResist = 0.35f;
			NPC.aiStyle = 3;
			AIType = NPCID.ArmedZombie;
			AnimationType = NPCID.ArmedZombie;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Cavern.Chance * (Main.hardMode ? 0.01f : 0.04f);
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			if (Main.hardMode) NPC.lifeMax = NPC.lifeMax * 2;
		}

		double bFrameCounter = 0;
		int bFrameY = 0;

		public ref float SpawnedGem => ref NPC.ai[1];

		public bool DecidedSpawnedGem
		{
			get => NPC.localAI[0] == 1f;
			set => NPC.localAI[0] = value ? 1f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.IsABestiaryIconDummy)
			{
				AssExtensions.LoopAnimation(ref bFrameY, ref bFrameCounter, 8, 0, Main.npcFrameCount[NPC.type] - 1);
				NPC.frame.Y = bFrameY * frameHeight;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, minimumDropped: 10, maximumDropped: 30));
			AddAppearanceLoot(npcLoot, 1, ItemID.Amethyst);
			AddAppearanceLoot(npcLoot, 2, ItemID.Topaz);
			AddAppearanceLoot(npcLoot, 3, ItemID.Sapphire);
			AddAppearanceLoot(npcLoot, 4, ItemID.Emerald);
			AddAppearanceLoot(npcLoot, 5, ItemID.Ruby);
			AddAppearanceLoot(npcLoot, 6, ItemID.Diamond);
		}

		private static void AddAppearanceLoot(NPCLoot npcLoot, int index, int itemID)
		{
			//Actual drop
			var dropRule = new LeadingConditionRule(new MatchAppearanceCondition(1, index));
			dropRule.OnSuccess(ItemDropRule.Common(itemID, 1), hideLootReport: true);
			npcLoot.Add(dropRule);

			//Dummy for bestiary, technically not accurate because we use bell curve, don't want to replicate logic for that
			npcLoot.Add(ItemDropRule.ByCondition(new NeverTrueWithDescriptionCondition("Drops based on appearance"), itemID, chanceDenominator: 6));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("Animate stone statues that have wandered from an ancient tomb. Their eyes are valuable.")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				var entitySource = NPC.GetSource_Death();
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("StoneSoldierGore_01").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("StoneSoldierGore_02").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("StoneSoldierGore_03").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("StoneSoldierGore_04").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("StoneSoldierGore_04").Type, 1f);
			}

			for (int i = 0; i < 15; i++)
			{
				if (Main.rand.NextFloat() < 0.6f)
				{
					Dust.NewDust(NPC.position - new Vector2(-20, 0), (NPC.width - 10) / 2, NPC.height, 1, 0f, 0f, 50, new Color(255, 255, 255), 1f);
				}
			}
		}

		private float Gaussian(float x, float mean, float var = 1f)
		{
			return (float)((1 / Math.Sqrt(MathHelper.TwoPi * var)) * Math.Exp(-(x - mean) * (x - mean) / (2 * var)));
		}

		public override bool PreAI()
		{
			if (SpawnedGem == 0 && !DecidedSpawnedGem && Main.netMode != NetmodeID.MultiplayerClient)
			{
				float heightFactor = (float)(NPC.position.Y - (16f * Main.rockLayer)) / (float)((Main.maxTilesY - 200 - Main.rockLayer) * 16f) * 6;
				//0f == above rock layer
				//3f == falfway down a hellavator
				//6f == hell start
				heightFactor = heightFactor > 6f ? 6f : heightFactor;
				heightFactor = heightFactor < 0f ? 0f : heightFactor;

				float[] arr = new float[6]; // 0 to 5
				for (int j = 1; j < 7; j++) //1 to 6
				{
					arr[j - 1] = Gaussian(j, heightFactor, 2f);
					if (j > 1) arr[j - 1] += arr[j - 2]; //cumulative sum
				}

				float sum = arr[arr.Length - 1];

				for (int j = 1; j < 7; j++)
				{
					arr[j - 1] /= sum;
				}

				float rand = Main.rand.NextFloat();
				for (int j = 1; j < 7; j++)
				{
					if (rand <= arr[j - 1]) //arr[6] is 1 
					{
						SpawnedGem = j;
						break;
					}
				}

				DecidedSpawnedGem = true;
				NPC.netUpdate = true;
			}

			if (SpawnedGem != 0 && NPC.ai[3] == 1)
			{
				if (NPC.direction == 1) NPC.velocity.X += 0.09f; //0.02
				else NPC.velocity.X -= 0.09f;
			}

			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			//base sprite is 80x66
			//hitbox is 18x40
			int tex = (int)SpawnedGem;
			if (tex <= 0 || tex > 6)
				return;

			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/StoneSoldier_" + tex).Value;
			Vector2 stupidOffset = new Vector2(0f, -8f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			drawColor = new Color((int)(drawColor.R * 1.2f + 40), (int)(drawColor.G * 1.2f + 40), (int)(drawColor.B * 1.2f + 40));
			drawColor.R = Math.Max(drawColor.R, (byte)100);
			drawColor.G = Math.Max(drawColor.G, (byte)100);
			drawColor.B = Math.Max(drawColor.B, (byte)100);
			spriteBatch.Draw(texture, drawPos, new Rectangle?(texture.Bounds), drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
		}
	}
}
