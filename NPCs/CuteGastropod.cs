using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.NPCs.CuteSlimes;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.CuteSlimes)]
	public class CuteGastropod : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingSnake];
			Main.npcCatchable[NPC.type] = true;

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;

			NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<CuteSlimeShimmer>();

			NPCID.Sets.CountsAsCritter[NPC.type] = true; //Guide To Critter Companionship
		}

		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 42;
			DrawOffsetY = 4;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 14;
			NPC.noGravity = true;
			AIType = NPCID.FlyingSnake;
			AnimationType = NPCID.FlyingSnake;
			NPC.catchItem = ModContent.ItemType<CuteGastropodItem>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SlimePets.CuteSlimeSpawnChance(spawnInfo, SpawnConditionType.None, customFactor: !NPC.npcsFoundForCheckActive[ModContent.NPCType<CuteGastropod>()] ? SpawnCondition.OverworldHallow.Chance * 0.045f : 0f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.UIInfoProvider = new CritterUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPC.type]);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
			});
		}
	}
}
