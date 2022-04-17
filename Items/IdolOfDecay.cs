using AssortedCrazyThings.NPCs.Harvester;
using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.Bosses)]
	public class IdolOfDecay : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Idol Of Decay");
			Tooltip.SetDefault("Summons " + HarvesterBoss.name + "'s final form in the dungeon"
				+ "\nUnlimited uses!");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 1;
			Item.rare = 1;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(silver: 5);
			Item.UseSound = SoundID.Item44;
		}

		public override bool CanUseItem(Player player)
		{
			return !BabyHarvesterHandler.TryFindBabyHarvester(out _, out _) && !NPC.AnyNPCs(AssortedCrazyThings.harvester) && BabyHarvesterHandler.ValidPlayer(player);
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				int type = AssortedCrazyThings.harvester;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}
	}
}
