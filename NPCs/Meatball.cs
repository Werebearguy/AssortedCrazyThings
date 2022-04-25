using AssortedCrazyThings.Items.Pets;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.HostileNPCs)]
	public class Meatball : AssNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meatball");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
			Main.npcCatchable[NPC.type] = true;
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
			NPC.catchItem = (short)ModContent.ItemType<MeatballItem>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				for (int i = 0; i < damage / NPC.lifeMax * 100f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 30; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2 * hitDirection, -1f);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Crimson.Chance * (Main.hardMode ? 0.04f : 0.15f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				new FlavorTextBestiaryInfoElement("A mass of bloody flesh with a single eye. The eye appears to be struggling in an attempt to escape.")
			});
		}

		public override void OnKill()
		{
			if (Main.rand.NextBool(10))
			{
#if TML_2022_03
				int i = NPC.NewNPC(NPC.GetSpawnSource_NPCHurt(), (int)NPC.position.X, (int)NPC.position.Y - 16, ModContent.NPCType<MeatballsEye>());
#else
				int i = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y - 16, ModContent.NPCType<MeatballsEye>());
#endif
				if (Main.netMode == NetmodeID.Server && i < Main.maxNPCs)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae));
		}
	}
}
