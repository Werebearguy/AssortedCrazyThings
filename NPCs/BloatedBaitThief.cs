using AssortedCrazyThings.Base;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.FriendlyNPCs)]
	public class BloatedBaitThief : AssNPC
	{
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
			NPC.width = 48;
			NPC.height = 42;
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
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.raining)
			{
				return SpawnCondition.TownWaterCritter.Chance * 0.8f;
			}
			else
			{
				return SpawnCondition.TownWaterCritter.Chance * 0.5f;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.GoldWorm, chanceDenominator: 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.Worm, minimumDropped: 2, maximumDropped: 8));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			});
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
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("BaitThiefGore_1").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("BaitThiefGore_0").Type, 1f);
			}
		}

		public override void AI()
		{
			AssAI.ModifiedGoldfishAI(NPC, 400f);
		}

		public override bool CheckActive()
		{
			NPC.netSpam = 0;
			return base.CheckActive();
		}
	}
}
