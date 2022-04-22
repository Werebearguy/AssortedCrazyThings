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
	public class FairySlime : AssNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fairy Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
			Main.npcCatchable[NPC.type] = true;
			NPCID.Sets.CountsAsCritter[NPC.type] = true; //Guide To Critter Companionship
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 30;
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
			NPC.color = new Color(213, 196, 197, 100);
			NPC.catchItem = (short)ModContent.ItemType<FairySlimeItem>();
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return null; //TODO NPC return true
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (!projectile.GetOwner().CanNPCBeHitByPlayerOrPlayerProjectile(NPC, projectile))
			{
				return false;
			}

			//This logic is distinct from vanilla, as we return true and not null
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Color color = NPC.color;
			if (NPC.life > 0)
			{
				for (int i = 0; i < damage / NPC.lifeMax * 100f; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hitDirection, -1f, NPC.alpha, color);
				}
			}
			else
			{
				for (int i = 0; i < 40; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, 2 * hitDirection, -2f, NPC.alpha, color);
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldHallow.Chance * 0.015f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("Not wanting to lose to its flying neighbors, it has grown wing-like extentions. It can't fly, but it's happy regardless of this fact.")
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel));
		}
	}
}
