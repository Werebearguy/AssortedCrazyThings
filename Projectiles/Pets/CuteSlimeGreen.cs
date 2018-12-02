using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class CuteSlimeGreen : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Green Slime");
					Main.projFrames[projectile.type] = 10;
					Main.projPet[projectile.type] = true;
					drawOffsetX = -20;
					drawOriginOffsetX = 0;
					drawOriginOffsetY = -22;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.PetLizard);
					aiType = ProjectileID.PetLizard;
					projectile.scale = 0.9f;
					projectile.alpha = 75;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.CuteSlimeGreen = false;
						}
					if (modPlayer.CuteSlimeGreen)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}