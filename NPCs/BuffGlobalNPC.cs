using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace AssortedCrazyThings.NPCs
{
	[Content(ConfigurationSystem.AllFlags)]
	public class BuffGlobalNPC : AssGlobalNPC
	{
		public override bool InstancePerEntity => true;

		public bool soulBurn = false;

		public override void ResetEffects(NPC npc)
		{
			soulBurn = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (soulBurn)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}

				int doubledDps = 6 * 2;
				npc.lifeRegen -= doubledDps;
				damage = Math.Max(1, doubledDps / 4); //mirrored roughly from vanilla, avoids lots of "1" popups when it runs out
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (soulBurn)
			{
				if (Main.rand.Next(4) < 3)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, 135, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 2.5f);
					dust.noGravity = true;
					dust.velocity *= 1.8f;
					dust.velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						dust.noGravity = false;
						dust.scale *= 0.5f;
					}
				}

				Lighting.AddLight(npc.Center, 0.3f, 0.3f, 0.7f);

				float r = 0.65f;
				float g = 0.75f;
				float b = 1f;
				float a = 1f;
				drawColor = NPC.buffColor(drawColor, r, g, b, a);
			}
		}
	}
}
