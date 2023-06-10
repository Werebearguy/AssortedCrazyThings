using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class StingSlime : AssNPC
	{
		private const int TotalNumberOfThese = 2;

		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/StingSlime_0"; //use fixed texture
			}
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
			Main.npcCatchable[NPC.type] = true;

			NPCID.Sets.DebuffImmunitySets.Add(NPC.type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[1] {
					BuffID.Poisoned
				}
			});

			NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.ShimmerSlime;
			Items.RoyalGelGlobalItem.RoyalGelNoAggroNPCs.Add(NPC.type);
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
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
			NPC.catchItem = ModContent.ItemType<StingSlimeItem>();
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			Color color = AiTexture switch
			{
				0 => new Color(152, 140, 166, 100),
				_ => new Color(234, 173, 83, 100),
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
			return SpawnCondition.OverworldDayDesert.Chance * 0.3f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Stinger));
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/StingSlime_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, 4f + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetNPCColorTintedByBuffs(NPC.GetAlpha(drawColor)), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
		}
	}
}
