using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace AssortedCrazyThings.Base.SwarmDraw
{
	public abstract partial class SwarmDrawUnit : ICloneable
	{
		private byte stuckTimer = 0;

		public void SurroundingAI(float velocityFactor = 1f, float sway = 0.25f, float veloDelta = 0.1f, float veloLimitClose = 2f, float magnitude = 7f)
		{
			Vector2 between = -pos;
			float distSQ = between.LengthSquared();

			float swayDistance = 150f * sway;
			if (distSQ < swayDistance * swayDistance)
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
				if (distSQ > swayDistance * 2f * swayDistance * 2f)
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

			if (distSQ < 0.75f)
			{
				stuckTimer++;
				if (stuckTimer > 120)
				{
					stuckTimer = 0;
					vel *= (vel.SafeNormalize(Vector2.UnitY) * 3).RotatedByRandom(MathHelper.Pi);
				}
			}
			else
			{
				stuckTimer -= 2;
			}
		}
	}
}
