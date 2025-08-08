using AssortedCrazyThings.Base.ModSupport;
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
			OtherModCalls.RegisterBoomerang(Type, new BoomerangInfo(Item.shoot, 1));
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
				if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.npcsFoundForCheckActive[NPCID.WallofFlesh])
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.active && npc.type == NPCID.Guide)
						{
							if (npc.IsNPCValidForBestiaryKillCredit())
							{
								Main.BestiaryTracker.Kills.RegisterKill(npc);
							}

							var hit = new NPC.HitInfo
							{
								Knockback = 10,
								HitDirection = -npc.direction,
								InstantKill = true
							};
							npc.StrikeNPC(hit);
							if (Main.netMode != NetmodeID.SinglePlayer)
							{
								NetMessage.SendStrikeNPC(npc, hit);
							}

							NPC.SpawnWOF(Item.position);

							byte plr = Player.FindClosest(Item.position, Item.width, Item.height);
							Player player = Main.player[plr];
							Item.NewItem(player.GetSource_Misc("GuideVoodoorang"), player.getRect(), ModContent.ItemType<GuideVoodoorang>());

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
