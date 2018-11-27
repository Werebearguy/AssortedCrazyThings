using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class pet_frog_01 : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Lifelike Mechanical Frog");
					Main.projFrames[projectile.type] = 8;
					Main.projPet[projectile.type] = true;
					drawOriginOffsetY = 1;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.Bunny);
					aiType = ProjectileID.Bunny;
					projectile.width = 18;
					projectile.height = 20;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					player.bunny = false; // Relic from aiType
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.pet_frog_01 = false;
						}
					if (modPlayer.pet_frog_01)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}