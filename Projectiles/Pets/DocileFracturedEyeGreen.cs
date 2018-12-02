using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
	public class DocileFracturedEyeGreen : ModProjectile
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Docile Fractured Eye");
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
							modPlayer.DocileFracturedEyeGreen = false;
						}
					if (modPlayer.DocileFracturedEyeGreen)
						{
							projectile.timeLeft = 2;
						}
				}
		}
}