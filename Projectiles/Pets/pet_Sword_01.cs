using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Projectiles.Pets
{
	public class pet_Sword_01 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Sword");
			Main.projFrames[projectile.type] = 8;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.TikiSpirit);
			aiType = ProjectileID.TikiSpirit;
			projectile.width = 26;
			projectile.height = 50;
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
				modPlayer.pet_Sword_01 = false;
			}
			if (modPlayer.pet_Sword_01)
			{
				projectile.timeLeft = 2;
			}
		}
	}
}