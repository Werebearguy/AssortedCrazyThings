using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class pet_cuteslime_03 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cute Pink Slime");
			Main.projFrames[projectile.type] = 10;
			Main.projPet[projectile.type] = true;
			drawOffsetX = -20;
			drawOriginOffsetX = 0;
			drawOriginOffsetY = -28;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.PetLizard);
			aiType = ProjectileID.PetLizard;
			projectile.scale = 0.6f;
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
				modPlayer.pet_cuteslime_03 = false;
			}
			if (modPlayer.pet_cuteslime_03)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}