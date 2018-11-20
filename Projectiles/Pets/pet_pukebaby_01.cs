using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class pet_pukebaby_01 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lil' Ichy");
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BabyHornet);
			aiType = ProjectileID.BabyHornet;
			projectile.width = 34;
			projectile.height = 38;
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
				modPlayer.pet_pukebaby_01 = false;
			}
			if (modPlayer.pet_pukebaby_01)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}