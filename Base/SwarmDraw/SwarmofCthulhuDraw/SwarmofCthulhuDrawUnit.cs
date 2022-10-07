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
		public const string assetName = "AssortedCrazyThings/Base/SwarmDraw/SwarmofCthulhuDraw/SwarmofCthulhu";

		public SwarmofCthulhuDrawUnit() :
			base(ModContent.Request<Texture2D>(assetName), 4, 6, 0, null)
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
			SurroundingAI(veloLimitClose: 1.8f);

			rot = vel.ToRotation() - 0;
			if (dir == 1)
			{
				rot -= MathHelper.Pi;
			}
		}
	}
}
