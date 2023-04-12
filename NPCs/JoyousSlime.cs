using AssortedCrazyThings.Base;
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
	[Content(ContentType.FriendlyNPCs)]
	public class JoyousSlime : AssNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
			Main.npcCatchable[NPC.type] = true;
			NPCID.Sets.CountsAsCritter[NPC.type] = true; //Guide To Critter Companionship
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 32;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 25f;
			NPC.aiStyle = 1;
			AIType = NPCID.ToxicSludge;
			AnimationType = NPCID.ToxicSludge;
			NPC.friendly = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.alpha = 175;
			NPC.color = new Color(169, 141, 255, 100);
			NPC.catchItem = ModContent.ItemType<JoyousSlimeItem>();
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return player.CanBeHitByItemCritterLike(NPC);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return projectile.CanBeHitByProjectileCritterLike(NPC);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			Color color = NPC.color;
			if (NPC.life > 0)
			{
				for (int i = 0; i < hit.Damage / NPC.lifeMax * 100f; i++)
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
			return SpawnCondition.OverworldDay.Chance * 0.015f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel));
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("A passive variant of slime with a friendly face. At night, it tries to be close to an equally friendly face.")
			});
		}
	}
}
