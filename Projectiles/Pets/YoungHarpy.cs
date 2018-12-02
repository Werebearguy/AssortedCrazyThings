using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class YoungHarpy : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Young Harpy");
					Main.projFrames[projectile.type] = 4;
					Main.projPet[projectile.type] = true;
				}
			public override void SetDefaults()
				{
					projectile.CloneDefaults(ProjectileID.BabyHornet);
					aiType = ProjectileID.BabyHornet;
				}
			public override bool PreAI()
				{
					Player player = Main.player[projectile.owner];
					player.hornet = false; // Relic from aiType
					return true;
				}
			public override void AI()
				{
					Player player = Main.player[projectile.owner];
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
					if (player.dead)
						{
							modPlayer.YoungHarpy = false;
						}
					if (modPlayer.YoungHarpy)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}