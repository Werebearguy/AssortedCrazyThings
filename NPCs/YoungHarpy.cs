using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	[Content(ContentType.FriendlyNPCs)]
	public class YoungHarpy : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingSnake];
			Main.npcCatchable[NPC.type] = true;

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Position = new Vector2(0, 10f)
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;

			NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.Shimmerfly;
		}

		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 30;
			NPC.damage = 0;
			NPC.defense = 1;
			NPC.lifeMax = 5;
			NPC.friendly = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 14;
			NPC.noGravity = true;
			AIType = NPCID.FlyingSnake;
			AnimationType = NPCID.FlyingSnake;
			NPC.catchItem = ModContent.ItemType<YoungHarpyItem>();
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return player.CanBeHitByItemCritterLike(NPC);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return projectile.CanBeHitByProjectileCritterLike(NPC);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Sky.Chance * 0.05f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.UIInfoProvider = new CritterUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPC.type]);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
			});
		}
	}
}
