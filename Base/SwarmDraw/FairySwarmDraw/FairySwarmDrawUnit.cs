using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SwarmDraw.FairySwarmDraw
{
	public class FairySwarmDrawUnit : SwarmDrawUnit
	{
		private const string name = "AssortedCrazyThings/Base/SwarmDraw/FairySwarmDraw/FairySwarm_";

		public int TexIndex { get; private set; }

		public FairySwarmDrawUnit(int index) :
			base(ModContent.Request<Texture2D>(name + index), 4, 5, 16, ModContent.Request<Texture2D>(name + index + "_Glow"))
		{
			TexIndex = index;
		}

		public override int GetShader(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.cPet;
		}

		public override void Animate()
		{
			int baseSpeed = FrameSpeed;
			baseSpeed -= (int)(Math.Abs(vel.X) + Math.Abs(vel.Y));
			AssExtensions.LoopAnimationInt(ref frame, ref frameCounter, baseSpeed, 0, FrameCount - 1);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			Front = TexIndex % 2 == 0;

			vel *= 0.8f;
		}

		public override void AI(Vector2 center)
		{
			//if (pos.X > MaxDistanceX || pos.X < -MaxDistanceX)
			//{
			//    vel.X *= -1;
			//    Front = !Front;
			//}

			//if (pos.Y > MaxDistanceY || pos.Y < -MaxDistanceY)
			//{
			//    vel.Y *= -1;
			//    Front = !Front;
			//}
			float velocityFactor = 1f;
			float sway = 0.2f;
			float veloDelta = 0.1f;
			Vector2 between = -pos;
			float distance = between.LengthSquared();
			float magnitude = 7f;

			float swayDistance = 150f * sway;
			float veloLimitClose = 2f;
			if (distance < swayDistance * swayDistance)
			{
				if (Math.Abs(vel.X) > veloLimitClose || Math.Abs(vel.Y) > veloLimitClose)
				{
					vel *= 0.99f;
				}
				veloDelta = 0.01f;
				if (between.X < -veloLimitClose)
				{
					between.X = -veloLimitClose;
				}
				if (between.X > veloLimitClose)
				{
					between.X = veloLimitClose;
				}
				if (between.Y < -veloLimitClose)
				{
					between.Y = -veloLimitClose;
				}
				if (between.Y > veloLimitClose)
				{
					between.Y = veloLimitClose;
				}
			}
			else
			{
				if (distance > swayDistance * 2f * swayDistance * 2f)
				{
					veloDelta = 0.2f;
				}
				between.Normalize();
				between *= magnitude;
			}

			veloDelta *= velocityFactor;

			if (Math.Abs(between.X) > Math.Abs(between.Y))
			{
				if (vel.X < between.X)
				{
					vel.X += veloDelta;
					if (veloDelta > 0.05f && vel.X < 0f)
					{
						vel.X += veloDelta;
					}
				}
				if (vel.X > between.X)
				{
					vel.X += -veloDelta;
					if (veloDelta > 0.05f && vel.X > 0f)
					{
						vel.X += -veloDelta;
					}
				}
			}
			if (Math.Abs(between.X) <= Math.Abs(between.Y) || veloDelta == 0.05f)
			{
				if (vel.Y < between.Y)
				{
					vel.Y += veloDelta;
					if (veloDelta > 0.05f && vel.Y < 0f)
					{
						vel.Y += veloDelta;
					}
				}
				if (vel.Y > between.Y)
				{
					vel.Y += -veloDelta;
					if (veloDelta > 0.05f && vel.Y > 0f)
					{
						vel.Y += -veloDelta;
					}
				}
			}

			rot = vel.X * 0.1f;

			//Useless
			//Vector3 color = default;
			//switch (TexIndex)
			//{
			//    case 0:
			//        color = new Vector3(255, 103, 175);
			//        break;
			//    case 1:
			//        color = new Vector3(103, 205, 255);
			//        break;
			//    case 2:
			//        color = new Vector3(103, 255, 183);
			//        break;
			//    default:
			//        break;
			//}

			//if (color != default)
			//{
			//    Lighting.AddLight(pos + center, color * (0.5f / 255));
			//}
		}
	}
}
