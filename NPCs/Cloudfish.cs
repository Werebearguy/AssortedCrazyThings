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
	public class Cloudfish : AssNPC
	{
		public float scareRange = 200f;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Goldfish];
			Main.npcCatchable[NPC.type] = true;

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
			NPC.width = 38;
			NPC.height = 36;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.25f;
			NPC.aiStyle = -1;
			AIType = NPCID.Goldfish;  //Needed so the fish turns around for some reason
			AnimationType = NPCID.Goldfish;
			NPC.noGravity = true;
			NPC.catchItem = ItemID.Cloudfish;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneSkyHeight)
			{
				if (spawnInfo.Water)
				{
					return SpawnCondition.Sky.Chance * 4.2f;
				}
			}
			return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, minimumDropped: 10, maximumDropped: 20));
			npcLoot.Add(ItemDropRule.Common(ItemID.RainCloud, chanceDenominator: 10, minimumDropped: 10, maximumDropped: 20));
			npcLoot.Add(ItemDropRule.Common(ItemID.SnowCloudBlock, chanceDenominator: 15, minimumDropped: 10, maximumDropped: 20));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
			});
		}

		public override void AI()
		{
			AssAI.ModifiedGoldfishAI(NPC, scareRange);
		}

		public override bool CheckActive()
		{
			NPC.netSpam = 0;
			return base.CheckActive();
		}
	}
}
