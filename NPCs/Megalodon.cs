using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
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
	public class Megalodon : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Position = new Vector2(66, 9f), //Position on the icon
				PortraitPositionXOverride = 0, //Position on the portrait when clicked on
				PortraitPositionYOverride = 9f,
				IsWet = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;
		}

		public override void SetDefaults()
		{
			NPC.width = 150;
			NPC.height = 52;
			NPC.damage = 150;
			NPC.defense = 50;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 10000f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 16;
			AIType = NPCID.Shark;
			AnimationType = NPCID.Shark;
			NPC.noGravity = true;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.wet)
			{
				NPC.frameCounter -= 0.4;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!NPC.npcsFoundForCheckActive[NPC.type])
			{
				return SpawnCondition.Ocean.Chance * 0.0005f;
			}
			else return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			if (ContentConfig.Instance.DroppedPets)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MiniMegalodonItem>()));
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			//quickUnlock: true so only 1 kill is required to list everything about it
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPC.type], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
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
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_0").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_1").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_2").Type, 1f);
			}
		}
	}
}
