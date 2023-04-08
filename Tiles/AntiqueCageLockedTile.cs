using AssortedCrazyThings.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Tiles
{
	public class AntiqueCageLockedTile : AntiqueCageTileBase
	{
		public const int FrameCount = 4;

		public override void SafeSetStaticDefaults()
		{
			AnimationFrameHeight = FrameHeight;

			InteractableCageTypes.Add(Type);
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			int frameSpeed = 7;

			frameCounter++;
			if (frameCounter < 1 * frameSpeed)
			{
				frame = 0;
			}
			else if (frameCounter < 2 * frameSpeed)
			{
				frame = 0;
			}
			else if (frameCounter < 3 * frameSpeed)
			{
				frame = 1;
			}
			else if (frameCounter < 4 * frameSpeed)
			{
				frame = 2;
			}
			else if (frameCounter < 5 * frameSpeed)
			{
				frame = 2;
			}
			else if (frameCounter < 6 * frameSpeed)
			{
				frame = 1;
			}
			else
			{
				frame = 0;
				frameCounter = 0;
			}
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];

			int x = i - tile.TileFrameX / 18;
			int y = j - tile.TileFrameY % FrameHeight / 18;

			if (!player.ConsumeItem(ModContent.ItemType<AntiqueKey>()))
			{
				return true;
			}

			SoundEngine.PlaySound(SoundID.Unlock, new Vector2(x * 16, y * 16));

			for (int l = x; l < x + Width; l++)
			{
				for (int m = y; m < y + Height; m++)
				{
					Tile tile2 = Framing.GetTileSafely(l, m);
					if (tile2.HasTile && tile2.TileType == Type)
					{
						tile2.TileType = (ushort)ModContent.TileType<AntiqueCageUnlockedTile>();
					}
				}
			}

			NetMessage.SendTileSquare(-1, x + 1, y + 1, 3);

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconID = ModContent.ItemType<AntiqueKey>();
			player.cursorItemIconText = "";
			player.cursorItemIconEnabled = true;
		}
	}
}
