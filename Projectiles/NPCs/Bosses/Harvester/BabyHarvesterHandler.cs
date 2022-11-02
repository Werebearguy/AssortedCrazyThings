using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.Harvester;
using AssortedCrazyThings.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester
{
	[Content(ContentType.Bosses)]
	public class BabyHarvesterHandler : AssSystem
	{
		public static int WhoAmICache { get; private set; } = Main.maxProjectiles;
		public static bool HasWhoAmICache => WhoAmICache >= 0 && WhoAmICache < Main.maxProjectiles;

		//Set to false when there was a proper harvester room generated with all contents intact
		//Set to true once it spawned the proper way in case the player cancels the encounter so it can spawn naturally again
		public static bool CanHarvesterSpawnNaturally { get; set; } = true;

		private static bool CheckedInteractableCage { get; set; } = false;

		public override void OnWorldLoad()
		{
			CheckedInteractableCage = false;
			CanHarvesterSpawnNaturally = true;
		}

		public override void OnWorldUnload()
		{
			CheckedInteractableCage = false;
			CanHarvesterSpawnNaturally = true;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("CanHarvesterSpawnNaturally", (bool)CanHarvesterSpawnNaturally);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			CanHarvesterSpawnNaturally = tag.GetBool("CanHarvesterSpawnNaturally");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((bool)CanHarvesterSpawnNaturally);
		}

		public override void NetReceive(BinaryReader reader)
		{
			CanHarvesterSpawnNaturally = reader.ReadBoolean();
		}

		public override void PostUpdateProjectiles()
		{
			CheckInteractableCageToSetNaturalSpawnFlag();

			TryFindBabyHarvester(out _, out int whoAmICache, fromCache: false);
			WhoAmICache = whoAmICache;

			TrySpawnBabyHarvester();

			ValidateBabyHarvester();

			TryGiveSoulBuffToEnemies();
		}

		/// <summary>
		/// Returns true if NPC isn't in soulbuffblacklist or is a worm body or tail
		/// </summary>
		private static bool EligibleToReceiveSoulBuff(NPC npc)
		{
			if (Array.BinarySearch(AssortedCrazyThings.soulBuffBlacklist, npc.type) >= 0)
			{
				return false;
			}
			return !AssUtils.IsWormBodyOrTail(npc);
		}

		private static void TryGiveSoulBuffToEnemies()
		{
			if (TryFindBabyHarvester(out Projectile proj, out _) &&
				proj.ModProjectile is BabyHarvesterProj babyHarvester && babyHarvester.HasValidPlayerOwner)
			{
				GiveSoulBuffToNearbyNPCs(babyHarvester.Player, proj.Center);
			}
			else
			{
				int index = NPC.FindFirstNPC(AssortedCrazyThings.harvester);
				if (index <= -1)
				{
					return;
				}
				else
				{
					var harvesterCenter = Main.npc[index].Center;
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						Player player = Main.player[i];

						if (player.active && !player.dead)
						{
							GiveSoulBuffToNearbyNPCs(player, harvesterCenter);
						}
					}
				}
			}
		}

		private static void GiveSoulBuffToNearbyNPCs(Player player, Vector2 position)
		{
			if (!IsTurningInvalidPlayer(player, out _) && (ValidPlayer(player) || player.DistanceSQ(position) < 2880 * 2880)) //one and a half screens or in suitable location
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && !npc.SpawnedFromStatue &&
						npc.type != AssortedCrazyThings.harvester && npc.DistanceSQ(player.Center) < 2880 * 2880 &&
						!npc.GetGlobalNPC<HarvesterGlobalNPC>().shouldSoulDrop)
					{
						if (EligibleToReceiveSoulBuff(npc))
						{
							npc.AddBuff(ModContent.BuffType<SoulBuff>(), 60, true);
						}
					}
				}
			}
		}

		private static void CheckInteractableCageToSetNaturalSpawnFlag()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			if (CanHarvesterSpawnNaturally)
			{
				//No point checking if it can spawn naturally already
				return;
			}

			if (CheckedInteractableCage)
			{
				return;
			}
			CheckedInteractableCage = true;

			int startX = 0;
			int endX = Main.maxTilesX / 2 + 1;
			if (WorldGen.dungeonX > endX)
			{
				//For performance reasons, check only half the world, don't rely on WorldGen.dMinX etc
				startX = endX;
				endX = Main.maxTilesX;
			}

			static bool InteractableCageExists(int startX, int endX)
			{
				for (int i = startX; i < endX; i++)
				{
					for (int j = 0; j < Main.maxTilesY; j++)
					{
						Tile tile = Framing.GetTileSafely(i, j);
						if (tile.HasTile && Main.wallDungeon[tile.WallType] && AntiqueCageTileBase.IsTileInteractable(i, j))
						{
							return true;
						}
					}
				}

				return false;
			}

			if (!InteractableCageExists(startX, endX))
			{
				//If it couldn't spawn before, but no tile was found: fall back to natural spawning
				CanHarvesterSpawnNaturally = true;

				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
		}

		public static bool TryFindBabyHarvester(out Projectile proj, out int index, bool fromCache = true)
		{
			proj = null;
			index = Main.maxProjectiles;

			if (!fromCache)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile other = Main.projectile[i];

					if (other.active && ValidProjectile(other))
					{
						proj = other;
						index = i;
						break;
					}
				}
			}
			else if (HasWhoAmICache)
			{
				proj = Main.projectile[WhoAmICache];
				index = WhoAmICache;
			}

			return proj != null;
		}

		private static bool ValidProjectile(Projectile proj)
		{
			return proj.ModProjectile is BabyHarvesterProj;
		}

		/// <summary>
		/// This handles spawning/despawning, utilizing a delayed condition check to handle edge cases
		/// </summary>
		public static bool ValidPlayer(Player player)
		{
			return player.GetModPlayer<BabyHarvesterPlayer>().Valid;
		}

		/// <summary>
		/// True if the condition is about to turn false
		/// </summary>
		public static bool IsTurningInvalidPlayer(Player player, out int timeLeft)
		{
			return player.GetModPlayer<BabyHarvesterPlayer>().IsTurningInvalid(out timeLeft);
		}

		public static void TrySpawnBabyHarvester()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			if (HasWhoAmICache)
			{
				//Delete any possible duplicates
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];

					if (proj.active && i != WhoAmICache && ValidProjectile(proj))
					{
						//AssUtils.Print("deleted a duplicate");
						proj.Kill();
					}
				}

				//Do not spawn
				return;
			}

			if (!CanHarvesterSpawnNaturally)
			{
				//Do not spawn unless otherwise specified
				return;
			}

			if (!NPC.downedBoss3 || AssWorld.downedHarvester || NPC.AnyNPCs(AssortedCrazyThings.harvester))
			{
				//Do not spawn if skele isn't slain yet or harvester is already slain or alive
				return;
			}

			//No alive baby harvester, spawn
			ForceSpawnBabyHarvester("You hear a faint cawing from the dungeon.");
		}

		public static void ForceSpawnBabyHarvester(string message, Vector2? posOverride = null, Player playerOverride = null)
		{
			if (playerOverride != null)
			{
				Spawn(message, playerOverride, posOverride);
				return;
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];

				if (player.active && !player.dead && ValidPlayer(player))
				{
					Spawn(message, player);
					break;
				}
			}

			static void Spawn(string message, Player player, Vector2? posOverride = null)
			{
				//AssUtils.Print(Main.time + " spawning harvester");
				BabyHarvesterProj.Spawn(player, posOverride);
				AssWorld.Message(message, HarvesterBoss.deathColor);
			}
		}

		private static void ValidateBabyHarvester()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			if (!TryFindBabyHarvester(out Projectile proj, out _))
			{
				return;
			}

			if (!(proj.ModProjectile is BabyHarvesterProj babyHarvester && babyHarvester.HasValidPlayerOwner))
			{
				return;
			}

			//If current player dead or not suitable anymore
			Player playerowner = babyHarvester.Player;
			if (!playerowner.dead && ValidPlayer(playerowner))
			{
				return;
			}

			//Find new suitable player, reassign owner
			bool found = false;
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];

				if (player.active && !player.dead && ValidPlayer(player))
				{
					//AssUtils.Print($"{Main.time} assign new player to harvester from {babyHarvester.PlayerOwner} to {i}");
					babyHarvester.AssignPlayerOwner(i);
					found = true;
					break;
				}
			}

			//If not found, despawn
			if (!found)
			{
				//AssUtils.Print("despawning harvester");
				proj.Kill();
			}
		}
	}
}
