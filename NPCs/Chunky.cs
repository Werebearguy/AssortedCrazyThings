using AssortedCrazyThings.Items.Pets;
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
	public class Chunky : AssNPC
	{
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
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 26;
			NPC.damage = 7;
			NPC.defense = 2;
			NPC.lifeMax = 20;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 20f;
			NPC.knockBackResist = 0.25f;
			NPC.aiStyle = 1;
			AIType = NPCID.ToxicSludge;
			AnimationType = NPCID.ToxicSludge;
			NPC.catchItem = ModContent.ItemType<ChunkySlimeItem>();
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / NPC.lifeMax * 100f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 18, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 40; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 18, 2 * hit.HitDirection, -2f);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Corruption.Chance * (Main.hardMode ? 0.05f : 0.2f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
			});
		}

		public override void OnKill()
		{
			if (Main.rand.NextBool(10))
			{
				int i = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y - 16, ModContent.NPCType<ChunkysEye>());
				if (Main.netMode == NetmodeID.Server && i < Main.maxNPCs)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk));
		}
	}
}
