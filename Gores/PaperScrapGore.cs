using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Gores
{
	public class PaperScrapGore : ModGore
	{
        public override void SetStaticDefaults()
        {
			GoreID.Sets.DisappearSpeedAlpha[Type] = 10;
			GoreID.Sets.DisappearSpeed[Type] = 7;
		}

        public override void OnSpawn(Gore gore)
		{
			gore.velocity += new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
			gore.Frame = new SpriteFrame(1, 8, 0, (byte)Main.rand.Next(8));
			gore.frameCounter = (byte)Main.rand.Next(8);
			//UpdateType = 910;
		}

        public override bool Update(Gore gore)
        {
			if (++gore.frameCounter >= 8 && gore.velocity.Y > 0.2f)
			{
				gore.frameCounter = 0;
				int num14 = gore.Frame.CurrentRow / 4;
				if (++gore.Frame.CurrentRow >= 4 + num14 * 4)
					gore.Frame.CurrentRow = (byte)(num14 * 4);
			}

			Gore_UpdateLeaf(gore, 0.2f);

			return false;
        }

		public static void Gore_UpdateLeaf(Gore gore, float windFactor = 1f, bool disappearOnPositiveY = true)
		{
			if (gore.alpha > 255)
			{
				gore.active = false;
				return;
			}

			Vector2 value = gore.position + new Vector2(12f) / 2f - new Vector2(4f) / 2f;
			value.Y -= 4f;
			Vector2 vector = gore.position - value;
			if (gore.velocity.Y < 0f)
			{
				Vector2 v = new Vector2(gore.velocity.X, -0.2f);
				int num = 4;
				num = (int)((float)num * 0.9f);
				Point point = (new Vector2(num, num) / 2f + value).ToTileCoordinates();
				if (!WorldGen.InWorld(point.X, point.Y))
				{
					gore.active = false;
					return;
				}

				Tile tile = Main.tile[point.X, point.Y];
				if (tile == null)
				{
					gore.active = false;
					return;
				}

				int num2 = 6;
				Rectangle rectangle = new Rectangle(point.X * 16, point.Y * 16 + (int)tile.LiquidAmount / 16, 16, 16 - (int)tile.LiquidAmount / 16);
				Rectangle value2 = new Rectangle((int)value.X, (int)value.Y + num2, num, num);
				bool flag = tile != null && tile.LiquidAmount > 0 && rectangle.Intersects(value2);
				if (flag)
				{
					if (tile.LiquidType == LiquidID.Honey)
					{
						v.X = 0f;
					}
					else if (tile.LiquidType == LiquidID.Lava)
					{
						gore.active = false;
						for (int i = 0; i < 5; i++)
						{
							Dust.NewDust(gore.position, num, num, 31, 0f, -0.2f);
						}
					}
					else
					{
						v.X = Main.WindForVisuals * windFactor;
					}

					if ((double)gore.position.Y > Main.worldSurface * 16.0)
						v.X = 0f;
				}

				if (!WorldGen.SolidTile(point.X, point.Y + 1) && !flag)
				{
					gore.velocity.Y = 0.1f;
					if (disappearOnPositiveY)
                    {
						gore.timeLeft = 0;
						gore.alpha += 20;
					}
				}

				v = Collision.TileCollision(value, v, num, num);
				if (flag)
					gore.rotation = v.ToRotation() + (float)Math.PI / 2f;

				v.X *= 0.94f;
				if (!flag || ((double)v.X > -0.01 && (double)v.X < 0.01))
					v.X = 0f;

				if (gore.timeLeft > 0)
					gore.timeLeft -= GoreID.Sets.DisappearSpeed[gore.type];
				else
					gore.alpha += GoreID.Sets.DisappearSpeedAlpha[gore.type];

				gore.velocity.X = v.X;
				gore.position.X += gore.velocity.X;
				return;
			}

			gore.velocity.Y += (float)Math.PI / 180f;
			Vector2 vector2 = new Vector2(Vector2.UnitY.RotatedBy(gore.velocity.Y).X * 1f, Math.Abs(Vector2.UnitY.RotatedBy(gore.velocity.Y).Y) * 1f);
			int num3 = 4;
			if ((double)gore.position.Y < Main.worldSurface * 16.0)
				vector2.X += Main.WindForVisuals * 4f * windFactor;

			Vector2 value3 = vector2;
			vector2 = Collision.TileCollision(value, vector2, num3, num3);
			Vector4 vector3 = Collision.SlopeCollision(value, vector2, num3, num3, 1f);
			gore.position.X = vector3.X;
			gore.position.Y = vector3.Y;
			vector2.X = vector3.Z;
			vector2.Y = vector3.W;
			gore.position += vector;
			if (vector2 != value3)
				gore.velocity.Y = -1f;

			Point point2 = (new Vector2(gore.Width, gore.Height) * 0.5f + gore.position).ToTileCoordinates();
			if (!WorldGen.InWorld(point2.X, point2.Y))
			{
				gore.active = false;
				return;
			}

			Tile tile2 = Main.tile[point2.X, point2.Y];
			if (tile2 == null)
			{
				gore.active = false;
				return;
			}

			int num4 = 6;
			Rectangle rectangle2 = new Rectangle(point2.X * 16, point2.Y * 16 + (int)tile2.LiquidAmount / 16, 16, 16 - (int)tile2.LiquidAmount / 16);
			Rectangle value4 = new Rectangle((int)value.X, (int)value.Y + num4, num3, num3);
			if (tile2 != null && tile2.LiquidAmount > 0 && rectangle2.Intersects(value4))
				gore.velocity.Y = -1f;

			gore.position += vector2;
			gore.rotation = vector2.ToRotation() + (float)Math.PI / 2f;
			if (gore.timeLeft > 0)
				gore.timeLeft -= GoreID.Sets.DisappearSpeed[gore.type];
			else
				gore.alpha += GoreID.Sets.DisappearSpeedAlpha[gore.type];
		}
	}
}
