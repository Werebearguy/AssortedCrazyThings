using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class MiniMegalodon : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Mini Megalodon");
					Main.projFrames[projectile.type] = 8;
					Main.projPet[projectile.type] = true;
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
							modPlayer.MiniMegalodon = false;
						}
					if (modPlayer.MiniMegalodon)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}