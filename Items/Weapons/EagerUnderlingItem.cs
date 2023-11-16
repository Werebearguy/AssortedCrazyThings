using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[LegacyName("GoblinUnderlingItem")]
	public class EagerUnderlingItem : GoblinUnderlingItem
	{
		public override int ProjType => ModContent.ProjectileType<EagerUnderlingProj>();

		public override int BuffType => ModContent.BuffType<EagerUnderlingBuff>();

		public override void Load()
		{
			base.Load();

			On_NPC.SetEventFlagCleared += DropItemIfPossible;
		}

		private static void DropItemIfPossible(On_NPC.orig_SetEventFlagCleared orig, ref bool eventFlag, int gameEventId)
		{
			//This is not clientside
			orig(ref eventFlag, gameEventId);

			if (gameEventId == GameEventClearedID.DefeatedGoblinArmy)
			{
				int itemType = ModContent.ItemType<EagerUnderlingItem>();

				static bool Condition(Player player, int itemType) => !player.HasItemWithBanks(itemType);

				if (Main.netMode == NetmodeID.Server)
				{
					int itemIndex = -1;
					for (int p = 0; p < Main.maxPlayers; p++)
					{
						Player player = Main.player[p];
						if (player.active && Condition(player, itemType))
						{
							int item = Item.NewItem(new EntitySource_WorldEvent(), player.Center, itemType, noBroadcast: true);
							itemIndex = item;
							NetMessage.SendData(MessageID.InstancedItem, p, -1, null, item);
							Main.item[item].active = false;
						}
					}

					if (itemIndex != -1)
					{
						Main.timeItemSlotCannotBeReusedFor[itemIndex] = 54000;
					}
				}
				else if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Player player = Main.LocalPlayer;
					if (Condition(player, itemType))
					{
						Item.NewItem(new EntitySource_WorldEvent(), player.Center, itemType);
					}
				}
			}
		}

		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
		}
	}
}
