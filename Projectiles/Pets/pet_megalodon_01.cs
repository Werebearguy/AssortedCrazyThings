using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class pet_megalodon_01 : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Little Megalodon");
					Main.projFrames[projectile.type] = 8;
					Main.projPet[projectile.type] = true;
					drawOffsetX = -45;
					drawOriginOffsetX = 0;
					drawOriginOffsetY = 0;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.EyeSpring);
					aiType = ProjectileID.EyeSpring;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					player.eyeSpring = false; // Relic from aiType
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.pet_megalodon_01 = false;
						}
					if (modPlayer.pet_megalodon_01)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}