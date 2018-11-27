using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class pet_cuteslime_06 : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Red Slime");
					Main.projFrames[projectile.type] = 10;
					Main.projPet[projectile.type] = true;
					drawOffsetX = -20;
					drawOriginOffsetX = 0;
					drawOriginOffsetY = -21;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.PetLizard);
					aiType = ProjectileID.PetLizard;
					projectile.scale = 1.025f;
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
							modPlayer.pet_cuteslime_06 = false;
						}
					if (modPlayer.pet_cuteslime_06)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}