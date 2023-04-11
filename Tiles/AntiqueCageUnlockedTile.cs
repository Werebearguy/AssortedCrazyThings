using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Tiles
{
	public class AntiqueCageUnlockedTile : AntiqueCageTileBase
	{
		public const int FrameCount = 3;

		public override void SafeSetStaticDefaults()
		{
			AnimationFrameHeight = FrameHeight;

			InteractableCageTypes.Add(Type);
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			int frameSpeed = 5;

			frameCounter++;
			if (frameCounter < 1 * frameSpeed)
			{
				frame = 0;
			}
			else if (frameCounter < 2 * frameSpeed)
			{
				frame = 1;
			}
			else if (frameCounter < 3 * frameSpeed)
			{
				frame = 2;
			}
			else if (frameCounter < 4 * frameSpeed)
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
			if (!BabyHarvesterHandler.ValidPlayer(player))
			{
				Main.NewText("Cannot open cage, not in dungeon", Color.OrangeRed);
				return true;
			}

			if (BabyHarvesterHandler.TryFindBabyHarvester(out _, out _) || NPC.AnyNPCs(AssortedCrazyThings.harvester))
			{
				Main.NewText("Soul Harvester is already alive, cage cannot be opened", Color.OrangeRed);
				return true;
			}

			Tile tile = Main.tile[i, j];

			int x = i - tile.TileFrameX / 18;
			int y = j - tile.TileFrameY % FrameHeight / 18;

			Vector2 spawnPos = new Vector2(x * 16, y * 16) + new Vector2(16 * 3) / 2;
			SpawnFromCage(player, spawnPos);

			for (int l = x; l < x + Width; l++)
			{
				for (int m = y; m < y + Height; m++)
				{
					Tile tile2 = Framing.GetTileSafely(l, m);
					if (tile2.HasTile && tile2.TileType == Type)
					{
						tile2.TileType = (ushort)ModContent.TileType<AntiqueCageOpenTile>();
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
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type);
			player.cursorItemIconText = "";
			player.cursorItemIconEnabled = true;
		}

		public static void SpawnFromCage(Player player, Vector2 spawnPos, bool request = true)
		{
			BabyHarvesterHandler.CanHarvesterSpawnNaturally = true;

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				BabyHarvesterHandler.ForceSpawnBabyHarvester(AssWorld.SoulHarvesterBabyCageAppear, spawnPos, player);
			}

			if (!request && Main.netMode != NetmodeID.Server)
			{
				SoundEngine.PlaySound(SoundID.Unlock, spawnPos);
				Rectangle dustRect = Utils.CenteredRectangle(spawnPos, new Vector2(16 * 3));
				for (int k = 0; k < 10; k++)
				{
					Dust dust = Dust.NewDustDirect(dustRect.TopLeft(), dustRect.Width, dustRect.Height, 135, 0, 0, 0, default(Color), 2f);
					dust.noGravity = true;
					dust.velocity = spawnPos - dust.position;
					dust.velocity.Normalize();
					dust.velocity *= -5f;
				}
			}

			if (Main.netMode != NetmodeID.SinglePlayer && request)
			{
				ModPacket packet = AssUtils.Instance.GetPacket();
				packet.Write((byte)AssMessageType.HarvesterSpawnFromCage);
				packet.Write((byte)player.whoAmI);
				packet.WriteVector2(spawnPos);
				packet.Send();
			}
		}
	}
}
