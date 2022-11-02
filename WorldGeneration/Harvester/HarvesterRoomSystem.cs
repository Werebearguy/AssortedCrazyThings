using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace AssortedCrazyThings.WorldGeneration.Harvester
{
	[Content(ContentType.Bosses)]
	public class HarvesterRoomSystem : AssSystem
	{
		//Metadata: Will be saved in twld for debugging/future use
		public static Point AntiqueCageGenLocation { get; set; } = Point.Zero;

		public override void OnWorldLoad()
		{
			AntiqueCageGenLocation = Point.Zero;
		}

		public override void OnWorldUnload()
		{
			AntiqueCageGenLocation = Point.Zero;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("AntiqueCageGenLocation", new List<int> { AntiqueCageGenLocation.X, AntiqueCageGenLocation.Y });
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var antiqueCageGenLocationList = tag.GetList<int>("AntiqueCageGenLocation");
			if (antiqueCageGenLocationList?.Count >= 2)
			{
				AntiqueCageGenLocation = new Point(antiqueCageGenLocationList[0], antiqueCageGenLocationList[1]);
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int dungeonIndex = tasks.FindIndex(t => t.Name == "Dungeon");
			if (dungeonIndex > -1)
			{
				tasks.Insert(dungeonIndex + 1, new HarvesterRoomGenPass());
			}
		}

		/*
		//The next two methods are debug only, remove on release
		public static bool JustPressed(Keys key)
		{
			return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
		}

		public override void PostUpdateWorld()
		{
			if (Main.GameUpdateCount % 60 == 0)
			{
				Main.NewText((int)Main.MouseWorld.X / 16 + " " + (int)Main.MouseWorld.Y / 16);
			}

			if (JustPressed(Keys.U))
			{
				var dungeonType = HarvesterRoomGenPass.GetDungeonType();
				HarvesterRoomGenPass.GenerateRoom((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, Main.LocalPlayer.direction, dungeonType.Value);
			}

			if (JustPressed(Keys.I))
			{
				bool isEmpty = HarvesterRoomGenPass.IsOpenAreaInDungeonWithFloorAndWall((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, 3, 3, new HashSet<int>
				{
					TileID.CrackedBlueDungeonBrick,
					TileID.CrackedGreenDungeonBrick,
					TileID.CrackedPinkDungeonBrick,
				}, out int wallSide);
				Main.NewText(isEmpty ? ("found, towards: " + wallSide) : "not found");
			}

			if (JustPressed(Keys.O))
			{
				bool success = HarvesterRoomGenPass.TryGetDoorPositionForRoom((int)Main.MouseWorld.X / 16 + 1, (int)Main.MouseWorld.Y / 16 + 3, 20, Main.LocalPlayer.direction, out Point doorPos, out int pathwayLength);

				Main.NewText(success ? ("found, " + doorPos + " " + pathwayLength) : "not found");
			}

			if (JustPressed(Keys.P))
			{
				//Starts from the door, going back to the open space
				HarvesterRoomGenPass.GeneratePathway((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, -Main.LocalPlayer.direction, 10, new HashSet<int>
				{
					TileID.CrackedBlueDungeonBrick,
					TileID.CrackedGreenDungeonBrick,
					TileID.CrackedPinkDungeonBrick,
				});
			}
		}
		*/
	}
}
