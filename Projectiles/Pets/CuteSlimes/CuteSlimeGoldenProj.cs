using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets.CuteSlimes
{
	public class CuteSlimeGoldenProj : CuteSlimeBaseProj
	{
		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeGolden;

		public override void SafeSetDefaults()
		{
			Projectile.alpha = 0;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(lightColor, Color.White, 0.4f) * Projectile.Opacity;
		}

		public override void AI()
		{
			//Color color = new Color(204, 181, 72, 255);
			//Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), color.R / 255f * 1.1f, color.G / 255f * 1.1f, color.B / 255f * 1.1f); Not a light pet
			if (Projectile.velocity.LengthSquared() > 1f || !Main.rand.NextBool(2))
			{
				int offset = 4;
				Vector2 vector = Projectile.position + new Vector2(-offset, -offset);
				int width = Projectile.width + offset * 2;
				int height = Projectile.height + offset * 2;
				Dust dust = Dust.NewDustDirect(vector, width, height, 246, Alpha: 100);
				dust.noGravity = true;
				dust.noLightEmittence = true;
				dust.velocity *= 0.2f;
				dust.scale = 1.3f;
			}
		}
	}
}
