using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class ChunkyandMeatball : ModProjectile
		{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Chunky and Meatball");
				Main.projFrames[projectile.type] = 3;
				Main.projPet[projectile.type] = true;
			}

			public override void SetDefaults()
			{
				projectile.CloneDefaults(ProjectileID.BabyEater);
				aiType = ProjectileID.BabyEater;
			}

			public override bool PreAI()
			{
				Player player = Main.player[projectile.owner];
				player.eater = false; // Relic from aiType
				return true;
			}

			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.ChunkyandMeatball = false;
						}
					if (modPlayer.ChunkyandMeatball)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}