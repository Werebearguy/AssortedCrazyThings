using AssortedCrazyThings.NPCs.DropConditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class WalkingTombstoneGolden : AssNPC
	{
		private const int TotalNumberOfThese = 5;

		/*
        * 0 = Tombstone
        * 1 = CrossGraveMarker
        * 2 = GraveMarker
        * 3 = Gravestone
        * 4 = Headstone
        */

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/WalkingTombstoneGolden_0"; //use fixed texture
			}
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Crab];

			NPCID.Sets.DebuffImmunitySets[NPC.type] = new NPCDebuffImmunityData()
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Confused
				}
			};
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 46;
			NPC.damage = 0;
			NPC.defense = 10;
			NPC.lifeMax = 40;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 75f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3;
			AIType = NPCID.Crab;
			AnimationType = NPCID.Crab;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneGraveyard ? 0.15f : 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			AddAppearanceLoot(npcLoot, 0, ItemID.RichGravestone2);
			AddAppearanceLoot(npcLoot, 1, ItemID.RichGravestone1);
			AddAppearanceLoot(npcLoot, 2, ItemID.RichGravestone3);
			AddAppearanceLoot(npcLoot, 3, ItemID.RichGravestone4);
			AddAppearanceLoot(npcLoot, 4, ItemID.RichGravestone5);
		}

		private static void AddAppearanceLoot(NPCLoot npcLoot, int index, int itemID)
		{
			//Actual drop
			var dropRule = new LeadingConditionRule(new MatchAppearanceCondition(1, index));
			dropRule.OnSuccess(ItemDropRule.Common(itemID, 1), hideLootReport: true);
			npcLoot.Add(dropRule);

			//Dummy for bestiary
			npcLoot.Add(ItemDropRule.ByCondition(new NeverTrueWithDescriptionCondition("Drops based on appearance"), itemID, chanceDenominator: 5));
		}

		public override void PostAI()
		{
			if (NPC.HasValidTarget && Main.player[NPC.target] is Player player && !player.ZoneGraveyard)
			{
				if (NPC.velocity.X > 0 || NPC.velocity.X < 0)
				{
					NPC.velocity.X = 0;
				}
				NPC.ai[0] = 1f;
				NPC.direction = 1;
				NPC.frameCounter = 0;
			}

			if (NPC.velocity.Y < 0)
			{
				NPC.velocity.Y = 0;
			}
		}

		public ref float AiTexture => ref NPC.ai[1];

		public bool DecidedAiTexture
		{
			get => NPC.localAI[0] == 1f;
			set => NPC.localAI[0] = value ? 1f : 0f;
		}

		public override bool PreAI()
		{
			if (AiTexture == 0 && !DecidedAiTexture && Main.netMode != NetmodeID.MultiplayerClient)
			{
				AiTexture = Main.rand.Next(TotalNumberOfThese);

				DecidedAiTexture = true;
				NPC.netUpdate = true;
			}

			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/WalkingTombstoneGolden_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
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
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("WalkingTombstoneGore_01").Type, 1f);
			}
		}
	}
}
