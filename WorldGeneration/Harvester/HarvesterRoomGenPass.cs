using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Items;
using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using AssortedCrazyThings.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AssortedCrazyThings.WorldGeneration.Harvester
{
	public class HarvesterRoomGenPass : GenPass
	{
		public const int RoomWidth = 23; //Includes the RoomWallThickness thick sides, where the door is on the edge of
		public const int RoomHeight = 13; //Includes the RoomWallThickness thick floor/ceiling
		public const int RoomWallThickness = 2;

		public enum DungeonType
		{
			Blue = 0,
			Green = 1,
			Pink = 2
		}

		public HashSet<int> crackedDungeonBrickTypes;

		public HarvesterRoomGenPass() : base("Soul Harvester Room", 1f)
		{
			crackedDungeonBrickTypes = new HashSet<int>();
			for (int i = 0; i < TileLoader.TileCount; i++)
			{
				if (TileID.Sets.CrackedBricks[i])
				{
					crackedDungeonBrickTypes.Add(i);
				}
			}
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			var logger = AssUtils.Instance.Logger;
			logger.Info("Start generating " + Name);
			var dungeonType = GetDungeonType();

			if (dungeonType == null)
			{
				logger.Warn("No dungeon entrance found: Not generating " + Name);
				return;
			}

			GetScanAreaParams(out int startX, out int endX, out int startY, out int endY);

			//Scan from top left to bottom right, by row, that way it prioritizes spawning close to the top
			int iterations = 0;
			for (int y = startY; y < endY; y++)
			{
				for (int x = startX; x < endX; x++)
				{
					iterations++;
					//Find open (air, nonsolid, cracked bricks, ...) 3x3 area in the dungeon (where floor elevation below it can only change 1)
					if (!IsOpenAreaInDungeonWithFloorAndWall(x, y, 3, 3, crackedDungeonBrickTypes, out int wallSide))
					{
						continue;
					}

					//Scan to the sides, looking for places in the wall to fit the room
					if (!TryGetDoorPositionForRoom(x + 1, y + 3, 20, wallSide, out Point doorPos, out int pathwayLength))
					{
						continue;
					}
					//logger.Info($"Found room space originating from ({x},{y}) for door {doorPos} facing {wallSide}, path length: {pathwayLength}");

					logger.Info($"Generate room after {iterations} iterations");
					bool success = GenerateRoom(doorPos.X, doorPos.Y, wallSide, dungeonType.Value);
					if (success)
					{
						logger.Info($"Successfully generated all necessary contents of the room");
					}

					if (pathwayLength > 0)
					{
						//Starts from the door, going back to the open space
						GeneratePathway(doorPos.X, doorPos.Y, -wallSide, pathwayLength, crackedDungeonBrickTypes);
					}

					logger.Info("Finish generating " + Name);
					return;
				}
			}

			logger.Info($"Failed to generate the room, natural boss spawn will not be prevented");

			//Scanned entire area for room, but none found: GenerateRoom stores the cage position, so if the position is empty/nonexistant, this means room didn't gen
		}

		/// <summary>
		/// Generates the room, and an outside shell if necessary.
		/// <br/>Make sure no other structures, containers, or tile entities are in that area before calling this method!
		/// <br/>Returns true if all required tiles have been placed
		/// </summary>
		/// <param name="doorX">The x coordinate at which the door will be placed</param>
		/// <param name="doorY">The y coordinate at which the door will be placed (bottom, as if placed by the player)</param>
		/// <param name="direction">The direction the room is facing (away from the door). 1 means door on the left side</param>
		/// <param name="dungeonType">The dungeon color type</param>
		public static bool GenerateRoom(int doorX, int doorY, int direction, DungeonType dungeonType)
		{
			//Because the room is symmetrical, alot of things don't need to change
			bool faceLeft = direction >= 0;

			int topLeftX = faceLeft ? doorX : doorX - RoomWidth + 1;
			int topLeftY = doorY + RoomWallThickness - RoomHeight + 1;
			var area = new Rectangle(topLeftX, topLeftY, RoomWidth, RoomHeight);

			//Assuming the location is safe to clear and rebuild
			//Clear tiles/walls in that place
			for (int i = 0; i < RoomWidth; i++)
			{
				for (int j = 0; j < RoomHeight; j++)
				{
					int x = topLeftX + i;
					int y = topLeftY + j;

					Tile tile = Framing.GetTileSafely(x, y);
					WorldGen.KillTile(x, y, noItem: true);
					WorldGen.KillWall(x, y);
					tile.Clear(TileDataType.Liquid);
					//tile.Clear(TileDataType.Tile);
					//tile.Clear(TileDataType.Slope);
					//tile.Clear(TileDataType.Wall);
				}
			}

			#region Immediate Shell
			//Place "wall"
			int dungeonBrickType = dungeonType == DungeonType.Blue ? TileID.BlueDungeonBrick : dungeonType == DungeonType.Green ? TileID.GreenDungeonBrick : TileID.PinkDungeonBrick;
			for (int i = 0; i < RoomWidth; i++)
			{
				for (int j = 0; j < RoomHeight; j++)
				{
					if (i < RoomWallThickness || i >= RoomWidth - RoomWallThickness ||
						j < RoomWallThickness || j >= RoomHeight - RoomWallThickness)
					{
						PlaceTile(topLeftX + i, topLeftY + j, dungeonBrickType);
					}
				}
			}

			//Add sloped blocks in each corner
			static void AddCornerSlope(int x, int y, int type, SlopeType slopeType)
			{
				PlaceTile(x, y, type);
				Tile slopedTile = Framing.GetTileSafely(x, y);
				slopedTile.Slope = slopeType;
			}

			AddCornerSlope(topLeftX + 2, topLeftY + 2, dungeonBrickType, SlopeType.SlopeUpLeft);
			AddCornerSlope(topLeftX - 3 + RoomWidth, topLeftY + 2, dungeonBrickType, SlopeType.SlopeUpRight);
			AddCornerSlope(topLeftX + 2, topLeftY - 3 + RoomHeight, dungeonBrickType, SlopeType.SlopeDownLeft);
			AddCornerSlope(topLeftX - 3 + RoomWidth, topLeftY - 3 + RoomHeight, dungeonBrickType, SlopeType.SlopeDownRight);

			//Clear out space for the door and place door (and remove the sloped block on the door side)
			for (int i = 0; i < 3; i++)
			{
				int doorHeight = 3;
				for (int j = 0; j < doorHeight; j++)
				{
					int x = doorX + direction * i;
					int y = doorY - doorHeight + 1 + j;

					WorldGen.KillTile(x, y, noItem: true);
					Tile tile = Framing.GetTileSafely(x, y);
					tile.Clear(TileDataType.Liquid);
				}
			}

			Furniture.GetFurnitureVars(FurnitureSet.gothic, FurnitureType.door, out ushort doorID, out ushort doorStyle);
			WorldGen.PlaceDoor(doorX, doorY - 1, doorID, doorStyle);

			//Slope the block away from the door ceiling
			Tile slopedTile = Framing.GetTileSafely(doorX + direction * 1, doorY - 3);
			slopedTile.Slope = faceLeft ? SlopeType.SlopeUpLeft : SlopeType.SlopeUpRight;
			#endregion

			#region Walls
			int wallType = dungeonType == DungeonType.Blue ? WallID.BlueDungeonSlabUnsafe : dungeonType == DungeonType.Green ? WallID.GreenDungeonSlabUnsafe : WallID.PinkDungeonSlabUnsafe;
			int bottomStripWallType = dungeonType == DungeonType.Blue ? WallID.BlueDungeonTileUnsafe : dungeonType == DungeonType.Green ? WallID.GreenDungeonTileUnsafe : WallID.PinkDungeonTileUnsafe;
			int pillarWallType = dungeonType == DungeonType.Blue ? WallID.BlueDungeonUnsafe : dungeonType == DungeonType.Green ? WallID.GreenDungeonUnsafe : WallID.PinkDungeonUnsafe;

			for (int i = 0; i < RoomWidth; i++)
			{
				for (int j = 0; j < RoomHeight; j++)
				{
					//Fill everything up with walls
					PlaceWall(topLeftX + i, topLeftY + j, wallType);

					//Place bottom strip
					if (i < RoomWidth - RoomWallThickness && j == RoomHeight - RoomWallThickness - 1)
					{
						PlaceWall(topLeftX + i, topLeftY + j, bottomStripWallType);
					}

					//Let pillar overwrite last
					if ((i == RoomWallThickness + 2 || i == RoomWallThickness + 3 || i == RoomWidth - RoomWallThickness - 3 || i == RoomWidth - RoomWallThickness - 4) &&
						(j >= RoomWallThickness || j < RoomHeight - RoomWallThickness))
					{
						PlaceWall(topLeftX + i, topLeftY + j, pillarWallType);
					}
				}
			}
			#endregion

			#region Decoration and cage&chest
			bool requiredTilesPlaced = true;

			FurnitureSet dungeonColorSet = dungeonType == DungeonType.Blue ? FurnitureSet.blueDungeon : dungeonType == DungeonType.Green ? FurnitureSet.greenDungeon : FurnitureSet.pinkDungeon;

			Furniture.GetFurnitureVars(dungeonColorSet, FurnitureType.candle, out ushort candleID, out ushort candleStyle);
			WorldGen.PlaceObject(topLeftX + 2, topLeftY + 4, TileID.Platforms, style: 9);
			WorldGen.PlaceObject(topLeftX + 3, topLeftY + 4, TileID.Platforms, style: 9);
			WorldGen.PlaceObject(topLeftX + 3, topLeftY + 3, candleID, style: candleStyle);

			WorldGen.PlaceObject(topLeftX - 3 + RoomWidth, topLeftY + 4, TileID.Platforms, style: 9);
			WorldGen.PlaceObject(topLeftX - 4 + RoomWidth, topLeftY + 4, TileID.Platforms, style: 9);
			WorldGen.PlaceObject(topLeftX - 4 + RoomWidth, topLeftY + 3, candleID, style: candleStyle);

			Furniture.GetFurnitureVars(dungeonColorSet, FurnitureType.table, out ushort tableID, out ushort tableStyle);
			var tablePos = new Point(topLeftX + RoomWidth / 2, topLeftY + RoomHeight - RoomWallThickness - 1);
			WorldGen.PlaceObject(tablePos.X, tablePos.Y, tableID, style: tableStyle);
			WorldGen.PlaceObject(tablePos.X, tablePos.Y - 2, candleID, style: candleStyle);

			Furniture.GetFurnitureVars(dungeonColorSet, FurnitureType.chair, out ushort chairID, out ushort chairStyle);
			var chairPos = new Point(tablePos.X - 2, tablePos.Y);
			WorldGen.PlaceObject(chairPos.X, chairPos.Y, chairID, style: chairStyle, direction: 1);
			WorldGen.PlaceObject(chairPos.X + 4, chairPos.Y, chairID, style: chairStyle, direction: -1);

			Furniture.GetFurnitureVars(dungeonColorSet, FurnitureType.lamp, out ushort lampID, out ushort lampStyle);
			var lampPos = new Point(tablePos.X - 3, tablePos.Y);
			WorldGen.PlaceObject(lampPos.X, lampPos.Y, lampID, style: lampStyle);
			WorldGen.PlaceObject(lampPos.X + 6, lampPos.Y, lampID, style: lampStyle);

			Furniture.GetFurnitureVars(dungeonColorSet, FurnitureType.bookcase, out ushort bookcaseID, out ushort bookcaseStyle);
			WorldGen.PlaceObject(tablePos.X + direction * 5, tablePos.Y, bookcaseID, style: bookcaseStyle);
			int chestIndex = WorldGen.PlaceChest(tablePos.X + (faceLeft ? 7 : -8), tablePos.Y, (ushort)ModContent.TileType<AntiqueChestTile>(), style: 1); //Place the locked variant
			if (chestIndex > -1)
			{
				Chest chest = Main.chest[chestIndex];
				chest.item[0].SetDefaults(ModContent.ItemType<AntiqueKey>());
			}
			else if (WorldGen.gen)
			{
				requiredTilesPlaced = false;
			}

			//PlaceObject doesn't work on chains for some reason
			var chainPos = new Point(topLeftX + RoomWidth / 2, topLeftY + 2);
			WorldGen.PlaceTile(chainPos.X, chainPos.Y, TileID.Chain);
			WorldGen.PlaceTile(chainPos.X, chainPos.Y + 1, TileID.Chain);

			int cageID = ModContent.TileType<AntiqueCageLockedTile>();
			var cagePos = new Point(chainPos.X, chainPos.Y + 3);
			WorldGen.PlaceObject(cagePos.X, cagePos.Y, (ushort)cageID);
			if (WorldGen.gen)
			{
				if (Framing.GetTileSafely(cagePos).TileType == cageID)
				{
					HarvesterRoomSystem.AntiqueCageGenLocation = cagePos;
				}
				else
				{
					requiredTilesPlaced = false;
				}
			}

			if (requiredTilesPlaced)
			{
				BabyHarvesterHandler.CanHarvesterSpawnNaturally = false;
			}
			#endregion

			#region Dungeon Shell
			//Create "bigger" area outside of the shell 9 tiles thick, if there is no dungeon wall, generate a dungeon brick there, and if it's not the edge of the shell, also a dungeon wall
			var outsideShellSize = InflateRectangleByShellSize(area);

			for (int i = 0; i < outsideShellSize.Width; i++)
			{
				for (int j = 0; j < outsideShellSize.Height; j++)
				{
					int x = outsideShellSize.X + i;
					int y = outsideShellSize.Y + j;
					if (area.Contains(new Point(x, y)))
					{
						continue;
					}

					Tile tile = Framing.GetTileSafely(x, y);
					if (Main.wallDungeon[tile.WallType])
					{
						continue;
					}

					if (!tile.HasTile || tile.TileType != dungeonBrickType)
					{
						WorldGen.KillTile(x, y, noItem: true);
						PlaceTile(x, y, dungeonBrickType);

						if (i > 0 && j > 0 && i < outsideShellSize.Width - 1 && j < outsideShellSize.Height - 1)
						{
							//Don't add walls on the very edges
							WorldGen.KillWall(i, j);
							PlaceWall(x, y, wallType);
						}
					}
				}
			}
			#endregion

			if (!WorldGen.gen)
			{
				WorldGen.RangeFrame(outsideShellSize.X, outsideShellSize.Y, outsideShellSize.X + outsideShellSize.Width, outsideShellSize.Y + outsideShellSize.Height);
			}
			else
			{
				GenVars.structures.AddProtectedStructure(area);
			}

			return requiredTilesPlaced;
		}

		/// <summary>
		/// Clears solid (and not blacklisted by <paramref name="tilesToTreatAsAir"/>) tiles as a <paramref name="pathwayLength"/> x 3 tunnel in a given <paramref name="direction"/> starting from <paramref name="doorX"/>:<paramref name="doorY"/>
		/// </summary>
		/// <param name="doorX">The x coordinate at which the door is placed</param>
		/// <param name="doorY">The y coordinate at which the door is placed (bottom, as if placed by the player)</param>
		/// <param name="direction">Which way to clear tiles</param>
		/// <param name="pathwayLength">How many tiles to clear on the x axis</param>
		/// <param name="tilesToTreatAsAir">Which tile types to ignore when clearing solid tiles</param>
		public static void GeneratePathway(int doorX, int doorY, int direction, int pathwayLength, HashSet<int> tilesToTreatAsAir)
		{
			//Start away from the door
			for (int i = 1; i < pathwayLength + 1; i++)
			{
				int x = doorX + direction * i;
				for (int y = doorY - 2; y <= doorY; y++) //Cover the entire door height
				{
					//Clear only solid, non-crumbling tiles
					Tile tile = Framing.GetTileSafely(x, y);
					int tileType = tile.TileType;
					//A solid tile (or one that should count as air)
					if (tile.HasTile && Main.tileSolid[tileType] && (tilesToTreatAsAir == null || !tilesToTreatAsAir.Contains(tileType)))
					{
						WorldGen.KillTile(x, y, noItem: true);
					}
				}
			}
		}

		/// <summary>
		/// Checks for a suitable "entry point" for the to-be-generated room
		/// </summary>
		/// <param name="topLeftX">Top left x of the area to check</param>
		/// <param name="topLeftY">Top left y of the area to check</param>
		/// <param name="sizeX">Width of the area to check</param>
		/// <param name="sizeY">Heighth of the area to check</param>
		/// <param name="tilesToTreatAsAir">Which tile types to ignore when checking for solid tiles</param>
		/// <param name="wallSide">Will be set to the direction a wall was found in</param>
		public static bool IsOpenAreaInDungeonWithFloorAndWall(int topLeftX, int topLeftY, int sizeX, int sizeY, HashSet<int> tilesToTreatAsAir, out int wallSide)
		{
			wallSide = 0;
			for (int x = topLeftX; x < topLeftX + sizeX; x++)
			{
				for (int y = topLeftY; y < topLeftY + sizeY; y++)
				{
					Tile tile = Framing.GetTileSafely(x, y);
					//Not in dungeon
					if (!Main.wallDungeon[tile.WallType])
					{
						return false;
					}

					int tileType = tile.TileType;
					//A solid tile (or one that should count as air)
					if (tile.HasTile && Main.tileSolid[tileType] && !Main.tileSolidTop[tileType] && (tilesToTreatAsAir == null || !tilesToTreatAsAir.Contains(tileType)))
					{
						//Main.NewText("solid tile within open area, abort");
						//Dust.QuickDust(new Point(x, y), Color.Red);
						return false;
					}
				}
			}

			//Check floor separately after open space is done, the bottom 2 tiles should combined include a row of solid tiles (with max 1 elevation change as a result of that)
			for (int x = topLeftX; x < topLeftX + sizeX; x++)
			{
				bool thisXHasFloor = false;
				int floorY = topLeftY + sizeY;
				for (int y = floorY; y < floorY + 2; y++)
				{
					Tile tile = Framing.GetTileSafely(x, y);
					//Floor should not be spikes (so that room entrance doesnt gen at the bottom of a spike pit)
					if (tile.HasTile && tile.TileType != TileID.Spikes && Main.tileSolid[tile.TileType])
					{
						//Main.NewText("solid tile for ground found");
						//Dust.QuickDust(new Point(x, y), Color.Green);

						//No need to scan further on x
						thisXHasFloor = true;
						break;
					}
				}

				if (!thisXHasFloor)
				{
					//Main.NewText("no solid ground found");
					return false;
				}
			}

			//Look for walls 3 tiles next to it (could be behind spikes) (-> any distance at which there are walls counts, even diagonal would work)
			for (int side = -1; side <= 1; side += 2)
			{
				bool allYHaveWalls = true;
				for (int y = topLeftY; y < topLeftY + sizeY; y++)
				{
					if (!allYHaveWalls)
					{
						break;
					}
					bool thisYHasWall = false;
					//Start adjacent to the empty area towards side
					int wallStartX = side == -1 ? topLeftX + -1 : topLeftX + sizeX;
					for (int i = 0; i <= 3; i++)
					{
						int x = wallStartX + side * i;
						Tile tile = Framing.GetTileSafely(x, y);
						//Dust.QuickDust(new Point(x, y), Color.White);
						int tileType = tile.TileType;
						//Treat spikes and doors as air
						if (tile.HasTile && tileType != TileID.Spikes && Main.tileSolid[tileType] && TileLoader.OpenDoorID(tile) == -1 && TileLoader.CloseDoorID(tile) == -1)
						{
							//Main.NewText("solid tile for wall found");
							//Dust.QuickDust(new Point(x, y), Color.Green);

							//No need to scan further on y
							thisYHasWall = true;
							break;
						}
					}

					if (!thisYHasWall)
					{
						allYHaveWalls = false;
						//Main.NewText("no walls found on side " + side);
						break;
					}
				}

				if (allYHaveWalls)
				{
					//The final return: everything lines up
					wallSide = side;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Scans <paramref name="maxDistance"/> coordinates in the <paramref name="direction"/> to check for suitable places to put the room in.
		/// </summary>
		/// <param name="floorX">The x of where the player would be standing (in an open area) to have direct horizontal access to the room</param>
		/// <param name="floorY">The y of where the player would be standing (in an open area) to have direct horizontal access to the room<</param>
		/// <param name="maxDistance">The maximum amount of coordinates to go in <paramref name="direction"/> to scan for room space</param>
		/// <param name="direction">Which side to check</param>
		/// <param name="doorPos">The position of the door (as it would be placed by a player)</param>
		/// <param name="pathwayLength">The actual amount of traversed coordinates required to reach <paramref name="doorPos"/></param>
		/// <returns></returns>
		public static bool TryGetDoorPositionForRoom(int floorX, int floorY, int maxDistance, int direction, out Point doorPos, out int pathwayLength)
		{
			doorPos = Point.Zero;
			//There should be no air in the area the room will be placed in if the air is infront of dungeon wall
			//There should be no chests or tile entities in the area

			//Ideally, if the entire initial area is fully covered in tiles, no pathway needs to be cleared, and the door can be placed directly at floorX/Y

			for (pathwayLength = 0; pathwayLength < maxDistance; pathwayLength++)
			{
				int xOff = pathwayLength * direction;
				int doorX = floorX + xOff;
				int topLeftX = direction == 1 ? doorX : doorX - RoomWidth + 1;
				int topLeftY = floorY + RoomWallThickness - RoomHeight + 1;
				var prelimDoorPos = new Point(doorX, floorY);

				if (pathwayLength > 10)
				{
					Tile wallTile = Framing.GetTileSafely(prelimDoorPos);
					if (Main.wallDungeon[wallTile.WallType])
					{
						//Door pos no longer within dungeon bounds
						continue;
					}
				}

				var area = new Rectangle(topLeftX, topLeftY, RoomWidth, RoomHeight);
				var outsideShellSize = InflateRectangleByShellSize(area);

				if (!NoChestsOrTEsInArea(outsideShellSize)) //Check larger area so that the added outside shell doesn't collide with these either
				{
					continue;
				}

				//Most cases will fail at the first check, only very rarely will the last one also fail if the others succeeded
				if (!NoUnwantedTilesInArea(area, 6) ||
					!NoUnwantedTilesInArea(area, 3) ||
					!NoUnwantedTilesInArea(area, 1))
				{
					continue;
				}

				doorPos = prelimDoorPos;
				//Main.NewText("door pos set");
				//Dust.QuickDust(doorPos, Color.Green);
				return true;
			}

			return false;
		}

		public static Rectangle InflateRectangleByShellSize(Rectangle area)
		{
			var outsideShellSize = area;
			int shellWidth = 10 - RoomWallThickness; //Vanilla walls are 9-11 thick
			outsideShellSize.Inflate(shellWidth, shellWidth);
			return outsideShellSize;
		}

		/// <summary>
		/// Gets the dungeon area and adjusts the top layer downwards
		/// </summary>
		public static void GetScanAreaParams(out int startX, out int endX, out int startY, out int endY)
		{
			startX = 0;
			if (GenVars.dMinX > 0)
			{
				startX = GenVars.dMinX;
			}
			endX = Main.maxTilesX;
			if (GenVars.dMaxX > startX)
			{
				endX = GenVars.dMaxX;
			}

			startY = (int)Main.worldSurface; //Dungeon guardian spawn line
			if (TryFindHighestLockedGoldChestInDungeon(out int lockedGoldChestY))
			{
				//This is roughly when rooms start appearing
				startY = lockedGoldChestY;
			}

			endY = Main.maxTilesY;
			if (GenVars.dMaxY > startY)
			{
				endY = GenVars.dMaxY;
			}
		}

		public static bool TryFindHighestLockedGoldChestInDungeon(out int chestY)
		{
			chestY = Main.maxTilesY;
			for (int i = 0; i < Main.maxChests; i++)
			{
				Chest chest = Main.chest[i];
				if (chest == null)
				{
					continue;
				}

				int x = chest.x;
				int y = chest.y;
				Tile tile = Framing.GetTileSafely(x, y);
				if (!Main.wallDungeon[tile.WallType])
				{
					continue;
				}

				if (tile.TileType != TileID.Containers)
				{
					continue;
				}

				if (tile.TileFrameX / (2 * 18) != Chests.goldLocked)
				{
					continue;
				}

				if (chest.y < chestY)
				{
					chestY = chest.y;
				}
			}

			return chestY != Main.maxTilesY;
		}

		public static bool NoUnwantedTilesInArea(Rectangle area, int holeSize)
		{
			int startX = area.X;
			int startY = area.Y;
			int width = area.Width;
			int height = area.Height;
			for (int x = startX; x < startX + width; x += holeSize)
			{
				for (int y = startY; y < startY + height; y += holeSize)
				{
					Tile tile = Framing.GetTileSafely(x, y);

					if (Main.wallDungeon[tile.WallType] && (!tile.HasTile || !Main.tileSolid[tile.TileType]))
					{
						//Main.NewText("air or non solid found, abort");
						//Dust.QuickDust(new Point(x, y), Color.Red);
						//In dungeon: if air or non-solid tile, don't place room
						return false;
					}

					//Outside of dungeon (usually happens to the edge areas): Don't care
				}
			}

			return true;
		}

		/// <summary>
		/// Checks for chests, tile entities, or other "problematic" tiles in the given area
		/// </summary>
		/// <param name="area">The area to check</param>
		public static bool NoChestsOrTEsInArea(Rectangle area)
		{
			//This really fucks with things (seems like things the dungeon overrides cover those structures which prevent the room from generating)
			//if (WorldGen.gen && !WorldGen.structures.CanPlace(area))
			//{
			//	return false;
			//}

			for (int i = 0; i < Main.maxChests; i++)
			{
				Chest chest = Main.chest[i];
				if (chest == null)
				{
					continue;
				}

				if (area.Contains(chest.x, chest.y))
				{
					//Main.NewText("chest found, abort");
					//Dust.QuickDust(new Point(chest.x, chest.y), Color.Red);
					//Overlaps with a chest, abort
					return false;
				}
			}

			var tesPositions = TileEntity.ByPosition.Keys;
			foreach (var pos in tesPositions)
			{
				if (area.Contains(pos.X, pos.Y))
				{
					//Main.NewText("tile entity found, abort");
					//Dust.QuickDust(new Point(pos.X, pos.Y), Color.Red);
					//Overlaps with a tile entity, abort
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Get the dungeon type (blue/green/pink). Returns null if no cultist/old man spawn exists
		/// </summary>
		public static DungeonType? GetDungeonType()
		{
			//Check the tiles below 
			if (Main.dungeonX <= 0 || Main.dungeonY <= 0)
			{
				//Something really wrong with the world, don't care about color
				return null;
			}

			//Tile below the cultist/old man spawn
			Tile floorTile = Framing.GetTileSafely(Main.dungeonX, Main.dungeonY + 1);

			if (floorTile.TileType == TileID.BlueDungeonBrick)
			{
				return DungeonType.Blue;
			}
			if (floorTile.TileType == TileID.GreenDungeonBrick)
			{
				return DungeonType.Green;
			}
			if (floorTile.TileType == TileID.PinkDungeonBrick)
			{
				return DungeonType.Pink;
			}

			return DungeonType.Blue;
		}

		/// <summary>
		/// Shorthand for forcibly clearing, then placing a tile
		/// </summary>
		private static void PlaceTile(int i, int j, int type, short frameX = 0, short frameY = 0)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			tile.Clear(TileDataType.Tile);
			tile.HasTile = true;
			tile.TileType = (ushort)type;
			tile.TileFrameX = frameX;
			tile.TileFrameY = frameY;
			//drunk world wall paint is applied later automatically by vanilla
		}

		/// <summary>
		/// Shorthand for forcibly clearing, then placing a wall
		/// </summary>
		private static void PlaceWall(int i, int j, int type)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			tile.Clear(TileDataType.Wall);
			tile.WallType = (ushort)type;
			//drunk world wall paint is applied later automatically by vanilla

			if (!WorldGen.gen)
			{
				WorldGen.SquareWallFrame(i, j);
			}
		}
	}
}
