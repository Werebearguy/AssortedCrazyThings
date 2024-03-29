using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Items.Accessories.Vanity;
using AssortedCrazyThings.Items.DroneUnlockables;
using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Items.Placeable;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DropConditions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
	[Content(ConfigurationSystem.AllFlags)]
	public class GeneralGlobalNPC : AssGlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (ContentConfig.Instance.Weapons)
			{
				//TODO convert this to a proper drop rule, see OnKill
				if (npc.type == NPCID.TheDestroyer)
				{
					LeadingConditionRule neverDropsRule = new LeadingConditionRule(new NotAllDronePartsUnlockedCondition());
					neverDropsRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DroneParts>()));
					npcLoot.Add(neverDropsRule);
				}
			}
		}

		public override void OnKill(NPC npc)
		{
			if (ContentConfig.Instance.Weapons)
			{
				if (npc.type == NPCID.TheDestroyer)
				{
					AssUtils.DropItemInstanced(npc, npc.Center, npc.Size, ModContent.ItemType<DroneParts>(),
					condition: delegate (NPC n, Player player)
					{
						return !DroneController.AllUnlocked(player);
					});
				}
			}

			GitgudData.Reset(npc);

			if (npc.boss)
			{
				for (int i = 0; i < npc.playerInteraction.Length; i++)
				{
					if (!npc.playerInteraction[i])
					{
						continue;
					}

					Player player = Main.player[i];

					if (!player.active)
					{
						continue;
					}

					player.GetModPlayer<AssPlayer>().SlainBoss(npc.type);
				}
			}
		}

		public override void ModifyShop(NPCShop shop)
		{
			int type = shop.NpcType;
			if (type == NPCID.PartyGirl)
			{
				if (ContentConfig.Instance.PlaceablesFunctional)
				{
					shop.Add(ModContent.ItemType<SlimeBeaconItem>(), Condition.DownedKingSlime);
				}
				if (ContentConfig.Instance.VanityAccessories)
				{
					shop.Add(ModContent.ItemType<SillyBalloonKit>());
				}
			}
		}

		public override void SetupTravelShop(int[] shop, ref int nextSlot)
		{
			if (ContentConfig.Instance.OtherPets && Main.rand.NextBool(10))
			{
				shop[nextSlot] = ModContent.ItemType<SuspiciousNuggetItem>();
				nextSlot++;
			}

			if (ContentConfig.Instance.OtherPets && Main.rand.NextBool(10))
			{
				shop[nextSlot] = ModContent.ItemType<StrangeRobotItem>();
				nextSlot++;
			}
		}
	}
}
