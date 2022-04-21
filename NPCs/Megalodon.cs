using AssortedCrazyThings.Items.Pets;
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
	public class Megalodon : AssNPC
	{
		public static string name = "Megalodon";
		public static string message = "A Megalodon is approaching! Get out of the ocean!";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
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
			if (!NPC.AnyNPCs(NPC.type))
			{
				return SpawnCondition.Ocean.Chance * 0.0005f;
			}
			else return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MiniMegalodonItem>()));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			//quickUnlock: true so only 1 kill is required to list everything about it
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPC.type], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Thought to be extinct, this ancient predator is as long-lived as its endless hunger.")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
#if TML_2022_03
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_0").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_2").Type, 1f);
#else
				var entitySource = NPC.GetSource_Death();
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_0").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_1").Type, 1f);
				Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("MegalodonGore_2").Type, 1f);
#endif
			}
		}
	}
}
