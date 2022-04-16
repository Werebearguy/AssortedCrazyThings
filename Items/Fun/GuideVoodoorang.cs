using AssortedCrazyThings.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
	[Content(ContentType.Weapons)]
	public class GuideVoodoorang : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guide Voodoorang");
			Tooltip.SetDefault("'Why are you like this?'");

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodenBoomerang);
			Item.width = 22;
			Item.height = 30;
			Item.rare = 3;

			Item.value = Item.sellPrice(silver: 2);
			Item.shoot = ModContent.ProjectileType<GuideVoodoorangProj>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one boomerang can be thrown out
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override void PostUpdate()
		{
			if (Item.lavaWet)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.AnyNPCs(NPCID.WallofFlesh))
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.active && npc.type == NPCID.Guide)
						{
							if (Main.netMode == NetmodeID.Server)
							{
								NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, i, 9999f, 10f, -npc.direction);
							}
							npc.StrikeNPCNoInteraction(9999, 10f, -npc.direction);
							NPC.SpawnWOF(Item.position);

							byte plr = Player.FindClosest(Item.position, Item.width, Item.height);
							Player player = Main.player[plr];
							Item.NewItem(player.GetItemSource_Misc(-1), player.getRect(), ModContent.ItemType<GuideVoodoorang>());

							//despawns upon wof spawn
							Item.TurnToAir();
							NetMessage.SendData(MessageID.SyncItem, number: Item.whoAmI);
							return;
						}
					}
				}
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.GuideVoodooDoll).AddTile(TileID.DemonAltar).Register();
		}
	}
}
