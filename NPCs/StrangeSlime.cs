using AssortedCrazyThings.NPCs.DropConditions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class StrangeSlime : AssNPC
	{
		private const int TotalNumberOfThese = 4;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/StrangeSlime_0"; //use fixed texture
			}
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];

			Items.RoyalGelGlobalItem.RoyalGelNoAggroNPCs.Add(NPC.type);
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 62;
			NPC.damage = 7;
			NPC.defense = 2;
			NPC.lifeMax = 25;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 25f;
			NPC.knockBackResist = 0.25f;
			NPC.aiStyle = 1;
			AIType = NPCID.ToxicSludge;
			AnimationType = NPCID.ToxicSludge;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			Color color = AiTexture switch
			{
				0 => new Color(171, 96, 193, 100),
				1 => new Color(240, 180, 4, 100),
				2 => new Color(84, 116, 8, 100),
				_ => new Color(25, 191, 83, 100),
			};
			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / NPC.lifeMax * 100f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, NPC.alpha, color);
				}
			}
			else
			{
				for (int i = 0; i < 40; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, 2 * hit.HitDirection, -2f, NPC.alpha, color);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldDaySlime.Chance * 0.001f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			AddAppearanceLoot(npcLoot, 0, ItemID.StrangePlant1);
			AddAppearanceLoot(npcLoot, 1, ItemID.StrangePlant2);
			AddAppearanceLoot(npcLoot, 2, ItemID.StrangePlant3);
			AddAppearanceLoot(npcLoot, 3, ItemID.StrangePlant4);
		}

		private static void AddAppearanceLoot(NPCLoot npcLoot, int index, int itemID)
		{
			//Actual drop
			var dropRule = new LeadingConditionRule(new MatchAppearanceCondition(1, index));
			dropRule.OnSuccess(ItemDropRule.Common(itemID, 1), hideLootReport: true);
			npcLoot.Add(dropRule);

			//Dummy for bestiary
			npcLoot.Add(ItemDropRule.ByCondition(new NeverTrueWithDescriptionCondition("Drops based on appearance"), itemID, chanceDenominator: 4));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("These peculiar slimes are host to strange plants, which grow for life. The slime always makes sure to keep it upright.")
			});
		}

		public ref float AiTexture => ref NPC.ai[1];

		public override bool PreAI()
		{
			if (AiTexture == 0 && NPC.localAI[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				AiTexture = Main.rand.Next(TotalNumberOfThese);

				NPC.localAI[0] = 1;
				NPC.netUpdate = true;
			}

			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/StrangeSlime_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
		}
	}
}
