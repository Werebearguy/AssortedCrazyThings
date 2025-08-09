using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class DemonEyeWandering : AssNPC
	{
		private const int TotalNumberOfThese = 2;

		/*WG = 0
        * WP = 1
        */
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/DemonEyeWandering_0"; //use fixed texture
			}
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.WanderingEye];

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true //Hides this NPC from the Bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 32;
			NPC.damage = 40;
			NPC.defense = 20;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 75f;
			NPC.knockBackResist = 0.8f;
			NPC.aiStyle = 2;
			AIType = NPCID.WanderingEye;
			AnimationType = NPCID.WanderingEye;
			Banner = Item.NPCtoBanner(NPCID.WanderingEye);
			BannerItem = Item.BannerToItem(Banner);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			var vanillaDrops = Main.ItemDropsDB.GetRulesForNPCID(NPCID.WanderingEye, false); // false is important here
			foreach (var dropRule in vanillaDrops)
			{
				npcLoot.Add(dropRule);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldNightMonster.Chance * (Main.hardMode ? 0.025f : 0f);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / (float)NPC.lifeMax * 50f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2 * hit.HitDirection, -1f);
				}

				var entitySource = NPC.GetSource_Death();
				switch ((int)AiTexture)//switch ((int)npc.altTexture)
				{
					case 0:
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyeGreenGore").Type, 1f);
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyeGreenGore").Type, 1f);
						break;
					case 1:
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyePurpleGore").Type, 1f);
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("WanderingEyePurpleGore").Type, 1f);
						break;
					default:
						break;
				}
			}
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/DemonEyeWandering_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, 0f); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetNPCColorTintedByBuffs(NPC.GetAlpha(drawColor)), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
		}
	}

	[Content(ContentType.HostileNPCs)]
	public class DemonEyeWanderingBestiarySystem : AssSystem
	{
		public override void PostSetupContent()
		{
			ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<DemonEyeWandering>()] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPCID.WanderingEye];
		}
	}
}
