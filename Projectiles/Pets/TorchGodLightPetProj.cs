using AssortedCrazyThings.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	[Content(ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilterOut: true)]
	public class TorchGodLightPetProj : SimplePetProjBase
	{
		private static Asset<Texture2D> flameAsset;

		public const int itemFlameCountMax = 5;
		public int itemFlameCount = itemFlameCountMax;
		public Vector2[] itemFlamePos = new Vector2[6];
		public readonly Color flameColor = new Color(75, 75, 75, 0);

		public float rotation2 = 0f;

		private const int yCounterMax = 140;
		private int yCounter; //Cos

		private const int rotCounterMax = 140;
		private int rotCounter; //Sin

		private const int scanTorchesTimerMax = 20;
		private int scanTorchesTimer = 0;

		private bool HasTorchTarget => TorchTarget != Point16.Zero;

		private Point16 TorchTarget
		{
			get => new Point16((short)Projectile.ai[0], (short)Projectile.ai[1]);
			set
			{
				Projectile.ai[0] = value.X;
				Projectile.ai[1] = value.Y;
			}
		}

		//Needs sync
		private bool CanPlaceTorches
		{
			get => Projectile.localAI[0] == 1f;
			set => Projectile.localAI[0] = value ? 1f : 0f;
		}

		private void ResetTorchTarget()
		{
			TorchTarget = Point16.Zero;
			Projectile.netUpdate = true;
		}

		public override void Load()
		{
			if (!Main.dedServ)
			{
				flameAsset = ModContent.Request<Texture2D>(Texture + "_Glowmask");
			}
		}

		public override void Unload()
		{
			flameAsset = null;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 7;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGhost);
			Projectile.aiStyle = -1;
			Projectile.width = 12;
			Projectile.height = 24;
			Projectile.alpha = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D image = TextureAssets.Projectile[Type].Value;
			Rectangle bounds = image.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

			float offY = (float)((Math.Cos(((float)yCounter / yCounterMax) * MathHelper.TwoPi) - 1) * 3);
			Vector2 stupidOffset = new Vector2(image.Width / 2, Projectile.height / 2 - Projectile.gfxOffY + offY);
			Vector2 rotationalOffset = new Vector2(0f, -8f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + stupidOffset - rotationalOffset;

			Vector2 origin = bounds.Size() / 2 - rotationalOffset;
			float rotation = Projectile.rotation;
			float scale = Projectile.scale;
			Main.EntitySpriteDraw(image, drawPos, bounds, lightColor, rotation, origin, scale, SpriteEffects.None, 0);

			image = flameAsset.Value;
			for (int i = 0; i < itemFlamePos.Length; i++)
			{
				Vector2 flameDrawPos = drawPos + itemFlamePos[i];
				Main.EntitySpriteDraw(image, flameDrawPos, bounds, flameColor, rotation, origin, scale, SpriteEffects.None, 0);
			}

			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((bool)CanPlaceTorches);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			CanPlaceTorches = reader.ReadBoolean();
		}

		public override void AI()
		{
			Player player = Projectile.GetOwner();
			PetPlayer modPlayer = player.GetModPlayer<PetPlayer>();
			if (player.dead)
			{
				modPlayer.TorchGodLightPet = false;
			}
			if (modPlayer.TorchGodLightPet)
			{
				Projectile.timeLeft = 2;


				rotCounter = rotCounter > rotCounterMax ? 0 : rotCounter + 1;
				yCounter = yCounter > yCounterMax ? 0 : yCounter + 1;

				SetItemFlamePos();

				if (CheckAllowPlacingTorches(player))
				{
					FindTorchTarget(player);
				}

				Lighting.AddLight(Projectile.Center, Color.Lerp(Color.White, Color.OrangeRed, 0.2f).ToVector3());

				if (HasTorchTarget && player.HasItem(ItemID.Torch))
				{
					PlaceTorchAI(player);
				}
				else
				{
					//Stay close to player
					AssAI.FlickerwickPetAI(Projectile, lightPet: true, lightDust: false, offsetX: 0f, offsetY: 0f);

					rotation2 = (float)((Math.Sin(((float)yCounter / yCounterMax) * MathHelper.TwoPi)) * 0.18f);
					Projectile.rotation = rotation2;
				}
			}
		}


		private bool CheckAllowPlacingTorches(Player player)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				CanPlaceTorches = Main.SmartCursorIsUsed;
				Projectile.netUpdate = true;
			}

			return CanPlaceTorches;
		}

		private void SetItemFlamePos()
		{
			//Copied from vanilla flame draw for player held torches
			if (Main.netMode != NetmodeID.Server)
			{
				itemFlameCount--;
				if (itemFlameCount <= 0)
				{
					itemFlameCount = itemFlameCountMax;
					for (int k = 0; k < itemFlamePos.Length; k++)
					{
						itemFlamePos[k].X = Main.rand.Next(-10, 11) * 0.15f;
						itemFlamePos[k].Y = Main.rand.Next(-10, 1) * 0.35f;
					}
				}
			}
		}

		private void FindTorchTarget(Player player)
		{
			if (HasTorchTarget || !CanPlaceTorches)
			{
				return;
			}

			scanTorchesTimer++;
			if (scanTorchesTimer >= scanTorchesTimerMax)
			{
				scanTorchesTimer = 0;

				const int radius = 8;

				int playerX = (int)(player.Center.X / 16);
				int playerY = (int)(player.Center.Y / 16);
				int rangeMinX = playerX - radius;
				int rangeMaxX = playerX + radius;
				int rangeMinY = playerY - radius;
				int rangeMaxY = playerY + radius;

				//If any torch in radius of player, don't place
				for (int i = rangeMinX; i <= rangeMaxX; i++)
				{
					for (int j = rangeMinY; j <= rangeMaxY; j++)
					{
						Tile tile = Framing.GetTileSafely(i, j);
						if (tile.TileType > 0 && TileID.Sets.Torch[tile.TileType])
						{
							return;
						}
					}
				}

				int intendedTorchItem = player.BiomeTorchHoldStyle(ItemID.Torch);
				bool notWaterTorch = !ItemID.Sets.WaterTorches[intendedTorchItem]; //Doesn't matter here since input param is default torch

				//Look for tiles to place a torch on
				List<Tuple<int, int>> targets = new();
				for (int i = rangeMinX; i <= rangeMaxX; i++)
				{
					for (int j = rangeMinY; j <= rangeMaxY; j++)
					{
						Tile tile = Framing.GetTileSafely(i, j);
						if (tile.HasTile /*&& !TileID.Sets.BreakableWhenPlacing[tile.TileType] && (!Main.tileCut[tile.TileType] || tile.TileType == TileID.ImmatureHerbs || tile.TileType == TileID.MatureHerbs)*/)
						{
							continue;
						}

						Tile tileLeft = Framing.GetTileSafely(i - 1, j);
						Tile tileRight = Framing.GetTileSafely(i + 1, j);
						Tile tileBottom = Framing.GetTileSafely(i, j + 1); //Does not place ontop of platforms (just like vanilla smart cursor)

						if ((!notWaterTorch || tile.LiquidAmount <= 0) &&
							(tile.WallType > 0 ||
								(tileLeft.HasTile && (tileLeft.Slope == 0 || (int)tileLeft.Slope % 2 != 1) &&
									(
										(Main.tileSolid[tileLeft.TileType] && !Main.tileNoAttach[tileLeft.TileType] && !Main.tileSolidTop[tileLeft.TileType] && !TileID.Sets.NotReallySolid[tileLeft.TileType])
										|| TileID.Sets.IsBeam[tileLeft.TileType] ||
										(WorldGen.IsTreeType(tileLeft.TileType) && WorldGen.IsTreeType(Framing.GetTileSafely(i - 1, j - 1).TileType) && WorldGen.IsTreeType(Framing.GetTileSafely(i - 1, j + 1).TileType))
									)
								) ||
								(tileRight.HasTile && (tileRight.Slope == 0 || (int)tileRight.Slope % 2 != 0) &&
									(
										(Main.tileSolid[tileRight.TileType] && !Main.tileNoAttach[tileRight.TileType] && !Main.tileSolidTop[tileRight.TileType] && !TileID.Sets.NotReallySolid[tileRight.TileType])
										|| TileID.Sets.IsBeam[tileRight.TileType] ||
										(WorldGen.IsTreeType(tileRight.TileType) && WorldGen.IsTreeType(Framing.GetTileSafely(i + 1, j - 1).TileType) && WorldGen.IsTreeType(Framing.GetTileSafely(i + 1, j + 1).TileType))
									)
								) ||
								(tileBottom.HasTile && Main.tileSolid[tileBottom.TileType] && !Main.tileNoAttach[tileBottom.TileType] && (!Main.tileSolidTop[tileBottom.TileType] || (TileID.Sets.Platforms[tileBottom.TileType] && tileBottom.Slope == 0))
								&& !TileID.Sets.NotReallySolid[tileBottom.TileType] && !tileBottom.IsHalfBlock && tileBottom.Slope == 0)
							)
							&& !TileID.Sets.Torch[tile.TileType])
						{
							targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}

				int finalX = -1;
				int finalY = -1;
				if (targets.Count > 0)
				{
					float maxDist = -1f;
					Tuple<int, int> finalTuple = targets[0];
					for (int m = 0; m < targets.Count; m++)
					{
						Tuple<int, int> tuple = targets[m];
						float dist = Vector2.Distance(new Vector2(tuple.Item1, tuple.Item2) * 16f + Vector2.One * 8f, player.Center);
						if (maxDist == -1f || (dist < maxDist && dist > 32))
						{
							maxDist = dist;
							finalTuple = tuple;
						}
					}

					if (Collision.InTileBounds(finalTuple.Item1, finalTuple.Item2, rangeMinX, rangeMinY, rangeMaxX, rangeMaxY))
					{
						finalX = finalTuple.Item1;
						finalY = finalTuple.Item2;
					}
				}

				if (finalX == -1 || finalY == -1)
				{
					return;
				}

				TorchTarget = new Point16(finalX, finalY);
			}
		}

		private void PlaceTorchAI(Player player)
		{
			AssAI.TeleportIfTooFar(Projectile, player.Center);

			Vector2 targetPos = new Vector2(TorchTarget.X, TorchTarget.Y) * 16;

			Vector2 toTargetPos = targetPos - Projectile.Center;
			float dist = toTargetPos.Length();
			Vector2 normalized = toTargetPos.SafeNormalize(Vector2.Zero);

			float acc;
			float speed;

			if (dist > 200)
			{
				acc = 3f;
				speed = 20f;
			}
			else if (dist > 64)
			{
				acc = 5f;
				speed = 15f;
			}
			else
			{
				acc = 8f;
				speed = 6f;
			}

			Projectile.velocity = (Projectile.velocity * (acc - 1) + normalized * speed) / acc;

			if (dist < 14f)
			{
				PlaceTorchAtTarget(player);
			}
		}

		private void PlaceTorchAtTarget(Player player)
		{
			if (!HasTorchTarget)
			{
				return;
			}

			if (player.ConsumeItem(ItemID.Torch))
			{
				if (Main.myPlayer == player.whoAmI)
				{
					short x = TorchTarget.X;
					short y = TorchTarget.Y;

					int type = TileID.Torches;
					int placeStyle = 0;
					if (player.UsingBiomeTorches)
					{
						player.BiomeTorchPlaceStyle(ref type, ref placeStyle);
					}

					if (WorldGen.PlaceTile(x, y, type, style: placeStyle))
					{
						NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, x, y, type, placeStyle);
					}
				}
			}

			ResetTorchTarget();
		}
	}
}
