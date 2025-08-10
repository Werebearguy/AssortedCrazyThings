using AssortedCrazyThings.Base;
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
	[Content(ContentType.FriendlyNPCs)]
	public class MutantFlinxfin : AssNPC
	{
		public float scareRange = 200f;

		private const int TotalNumberOfThese = 2;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/MutantFlinxfin_0"; //use fixed texture
			}
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				IsWet = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;
			NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.Shimmerfly;

			NPCID.Sets.CountsAsCritter[NPC.type] = true; //Guide To Critter Companionship
			//NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 32;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.25f;
			NPC.aiStyle = -1;
			AIType = NPCID.Goldfish; //Needed so the fish turns around for some reason
			AnimationType = NPCID.Goldfish;
			NPC.noGravity = true;
			NPC.catchItem = ItemID.MutantFlinxfin;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneSnow && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneDirtLayerHeight))
			{
				if (spawnInfo.Water)
				{
					float baseChance = 0.3f;
					if (NPC.CountNPCS(Type) >= 3)
					{
						baseChance = 0.1f;
					}
					return baseChance * (SpawnCondition.Cavern.Chance + SpawnCondition.Underground.Chance);
				}
			}
			return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.FlinxFur, 2, minimumDropped: 1, maximumDropped: 2));
		}

		public override void AI()
		{
			AssAI.ModifiedGoldfishAI(NPC, 200f);
		}

		public ref float AiTexture => ref NPC.ai[3];

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
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/MutantFlinxfin_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetNPCColorTintedByBuffs(NPC.GetAlpha(drawColor)), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
		}

		public override bool CheckActive()
		{
			NPC.netSpam = 0;
			return base.CheckActive();
		}
	}
}
