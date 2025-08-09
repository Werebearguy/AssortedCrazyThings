using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class MudSlime : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;

			NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.ShimmerSlime;
			Items.RoyalGelGlobalItem.RoyalGelNoAggroNPCs.Add(NPC.type);
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 30;
			NPC.damage = 32;
			NPC.defense = 6;
			NPC.lifeMax = 132;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.75f;
			NPC.aiStyle = 1;
			AIType = NPCID.BlueSlime;
			AnimationType = NPCID.BlueSlime;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			Color color = new Color(112, 82, 89, 100);
			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / (float)NPC.lifeMax * 50f; i++)
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
			//x3 as common as mimics
			return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 ? SpawnCondition.UndergroundJungle.Chance * 0.2f : 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
			});

			bestiaryEntry.UIInfoProvider = new MudSlimeInfoProvider(NPC.GetBestiaryCreditId(), false);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.MudBlock, minimumDropped: 5, maximumDropped: 10));
		}

		public override void PostAI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int start = (int)(NPC.BottomLeft.X / 16);
				int end = (int)(NPC.BottomRight.X / 16 + 1);
				for (int x = start; x < end; x++)
				{
					int y = (int)(NPC.Bottom.Y + 1) / 16;
					Tile tile = Main.tile[x, y];
					if (tile.HasTile && tile.TileType == TileID.Chlorophyte)
					{
						NPC.Transform(ModContent.NPCType<ChloroSlime>());
						break;
					}
				}
			}
		}
	}
}
