using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class CuteSlimeBlue : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Blue Slime");
					Main.projFrames[projectile.type] = 10;
					Main.projPet[projectile.type] = true;
					drawOffsetX = -20;
					drawOriginOffsetX = 0;
					drawOriginOffsetY = -20;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.PetLizard);
					aiType = ProjectileID.PetLizard;
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
							modPlayer.CuteSlimeBlue = false;
						}
					if (modPlayer.CuteSlimeBlue)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}