using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
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
			DisplayName.SetDefault("Young Harpy");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FlyingSnake];
			Main.npcCatchable[NPC.type] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Position = new Vector2(0, 10f)
			};
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;
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
			NPC.catchItem = (short)ModContent.ItemType<YoungHarpyItem>();
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return null; //TODO NPC return true
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type] || projectile.sentry || ProjectileID.Sets.SentryShot[projectile.type])
			{
				return false;
			}
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Sky.Chance * 0.05f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			//quickUnlock: true so only 1 kill is required to list everything about it
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[NPC.type], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("Harpies are not inherently hostile. If they make a childhood friend, they'll always be friends.")
			});
		}
	}
}
