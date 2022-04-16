using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SwarmDraw.SwarmofCthulhuDraw
{
	public class SwarmofCthulhuDrawUnit : SwarmDrawUnit
	{
		private const string name = "AssortedCrazyThings/Base/SwarmDraw/SwarmofCthulhuDraw/SwarmofCthulhu";

		public SwarmofCthulhuDrawUnit() :
			base(ModContent.Request<Texture2D>(name), 4, 6, 0, null)
		{

		}

		public override int GetShader(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.cPet;
		}

		public override Color GetColor(PlayerDrawSet drawInfo)
		{
			return drawInfo.colorArmorBody;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			vel *= 0.6f;
		}

		public override void AI(Vector2 center)
		{
			float velocityFactor = 1f;
			float sway = 0.25f;
			float veloDelta = 0.1f;
			Vector2 between = -pos;
			float distance = between.LengthSquared();
			float magnitude = 7f;

			float swayDistance = 150f * sway;
			float veloLimitClose = 1.8f;
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

			rot = vel.ToRotation() - 0;
			if (dir == 1)
			{
				rot -= MathHelper.Pi;
			}
		}
	}
}
