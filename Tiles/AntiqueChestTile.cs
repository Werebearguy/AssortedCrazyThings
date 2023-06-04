﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.Collections.Generic;
using AssortedCrazyThings.Items.Placeable;
using Terraria.Map;

namespace AssortedCrazyThings.Tiles
{
	[Content(ContentType.Bosses)]
	public class AntiqueChestTile : AssTile
	{
		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileOreFinderPriority[Type] = 500;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.BasicChest[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.AvoidedByNPCs[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.IsAContainer[Type] = true;
			TileID.Sets.FriendlyFairyCanLureTo[Type] = true;

			DustType = 1;
			AdjTiles = new int[] { TileID.Containers };

			// Names
			LocalizedText name = ModContent.GetInstance<AntiqueChestItem>().DisplayName;
			AddMapEntry(new Color(102, 115, 103), name, MapChestName);
			name = this.GetLocalization("MapEntry_Locked");
			AddMapEntry(new Color(147, 160, 163), name, MapChestName);

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] {
				TileID.MagicalIceBlock,
				TileID.Boulder,
				TileID.BouncyBoulder,
				TileID.LifeCrystalBoulder,
				TileID.RollingCactus
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
		}

		public override LocalizedText DefaultContainerName(int frameX, int frameY)
		{
			int option = /*frameX / 36*/0; //Only use the first style for the default name
			return Lang._mapLegendCache[MapHelper.TileToLookup(Type, option)];
		}

		public override ushort GetMapOption(int i, int j)
		{
			return (ushort)(Main.tile[i, j].TileFrameX / 36);
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override bool IsLockedChest(int i, int j)
		{
			return Main.tile[i, j].TileFrameX / 36 == 1;
		}

		public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
		{
			dustType = DustType;
			return true;
		}

		public override bool LockChest(int i, int j, ref short frameXAdjustment, ref bool manual)
		{
			int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
			//We need to return true only if the tile style is the unlocked variant of a chest that supports locking. 
			if (style == 0)
			{
				return true;
			}
			return false;
		}

		public static string MapChestName(string name, int i, int j)
		{
			int left = i;
			int top = j;
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}

			if (tile.TileFrameY != 0)
			{
				top--;
			}

			int chest = Chest.FindChest(left, top);
			if (chest < 0)
			{
				return Language.GetTextValue("LegacyChestType.0");
			}

			if (Main.chest[chest].name == "")
			{
				return name;
			}

			return name + ": " + Main.chest[chest].name;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
			int defaultType = TileLoader.GetItemDropFromTypeAndStyle(Type, 0);
			if (style == 0)
			{
				yield return new Item(defaultType);
			}
			if (style == 1)
			{
				//Style 1 is when locked. We want that tile style to drop the original item as well. Use the Chest Lock item to lock this chest.
				//No item places this in the locked style, so the automatic item drop is unknown, this is why GetItemDrops is necessary in this situation. 
				yield return new Item(defaultType);
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Chest.DestroyChest(i, j);
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}

			if (tile.TileFrameY != 0)
			{
				top--;
			}

			player.CloseSign();
			player.SetTalkNPC(-1);
			Main.npcChatCornerItem = 0;
			Main.npcChatText = "";
			if (Main.editChest)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
				Main.editChest = false;
				Main.npcChatText = string.Empty;
			}

			if (player.editedChestName)
			{
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
				player.editedChestName = false;
			}

			bool isLocked = Chest.IsLocked(left, top);
			if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
			{
				if (left == player.chestX && top == player.chestY && player.chest != -1)
				{
					player.chest = -1;
					Recipe.FindRecipes();
					SoundEngine.PlaySound(SoundID.MenuClose);
				}
				else
				{
					NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
					Main.stackSplit = 600;
				}
			}
			else
			{
				if (isLocked)
				{
					int key = ItemID.GoldenKey;
					if (player.ConsumeItem(key, includeVoidBag: true) && Chest.Unlock(left, top))
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, left, top);
						}
					}
				}
				else
				{
					int chest = Chest.FindChest(left, top);
					if (chest != -1)
					{
						Main.stackSplit = 600;
						if (chest == player.chest)
						{
							player.chest = -1;
							SoundEngine.PlaySound(SoundID.MenuClose);
						}
						else
						{
							SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
							player.OpenChest(left, top, chest);
						}

						Recipe.FindRecipes();
					}
				}
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}

			if (tile.TileFrameY != 0)
			{
				top--;
			}
			Tile topLeft = Main.tile[left, top];

			int chest = Chest.FindChest(left, top);
			player.cursorItemIconID = -1;
			if (chest < 0)
			{
				player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
			}
			else
			{
				string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language
				player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
				if (player.cursorItemIconText == defaultName)
				{
					int style = tile.TileFrameX / 36;
					player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
					if (style == 1)
					{
						player.cursorItemIconID = ItemID.GoldenKey;
					}

					player.cursorItemIconText = "";
				}
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}
