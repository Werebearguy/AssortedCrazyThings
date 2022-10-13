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
			SurroundingAI(sway: 0.2f);

			rot = vel.X * 0.1f;
		}
	}
}
