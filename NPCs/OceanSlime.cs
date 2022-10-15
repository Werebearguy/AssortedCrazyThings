using AssortedCrazyThings.Items.Pets;
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
	public class OceanSlime : AssNPC
	{
		private const int TotalNumberOfThese = 4;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/OceanSlime_0"; //use fixed texture
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ocean Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
			Main.npcCatchable[NPC.type] = true;

			Items.RoyalGelGlobalItem.RoyalGelNoAggroNPCs.Add(NPC.type);
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 36;
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
			NPC.catchItem = (short)ModContent.ItemType<OceanSlimeItem>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Color color = AiTexture switch
			{
				1 => new Color(217, 216, 255, 100),
				2 => new Color(98, 148, 143, 100),
				3 => new Color(254, 180, 246, 100),
				_ => new Color(65, 193, 247, 100),
			};
			if (NPC.life > 0)
			{
				for (int i = 0; i < damage / NPC.lifeMax * 100f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hitDirection, -1f, NPC.alpha, color);
				}
			}
			else
			{
				for (int i = 0; i < 40; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, 2 * hitDirection, -2f, NPC.alpha, color);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Ocean.Chance * 0.015f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			AddAppearanceLoot(npcLoot, 0, ItemID.Gel);
			AddAppearanceLoot(npcLoot, 1, ItemID.BlackInk);
			AddAppearanceLoot(npcLoot, 2, ItemID.SharkFin);
			AddAppearanceLoot(npcLoot, 3, ItemID.PinkGel);
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Slimes that are lost to the sea are faced with two options; adapt, or be eaten. Luckily, these slimes taste bad.")
			});
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/OceanSlime_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
		}
	}
}
