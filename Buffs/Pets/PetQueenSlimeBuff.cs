using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetQueenSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetQueenSlimeAirProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetQueenSlime;

		public override void SafeSetDefaults()
		{
			DisplayName.SetDefault("Playful Slimelings");
			Description.SetDefault("A trio of slimelings is following you");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetBool(player) = true;
			if (player.whoAmI == Main.myPlayer)
			{
				int air = ModContent.ProjectileType<PetQueenSlimeAirProj>();
				var spawnSource = player.GetSource_Buff(buffIndex);
				if (player.ownedProjectileCounts[air] <= 0)
				{
					Projectile.NewProjectile(spawnSource, player.Center.X, player.Center.Y, 0, -0.5f, air, 0, 0f, player.whoAmI);
				}

				int ground1 = ModContent.ProjectileType<PetQueenSlimeGround1Proj>();
				if (player.ownedProjectileCounts[ground1] <= 0)
				{
					Projectile.NewProjectile(spawnSource, player.Center.X - 20f, player.Center.Y, player.direction * -0.75f, 0f, ground1, 0, 0f, player.whoAmI);

				}

				int ground2 = ModContent.ProjectileType<PetQueenSlimeGround2Proj>();
				if (player.ownedProjectileCounts[ground2] <= 0)
				{
					Projectile.NewProjectile(spawnSource, player.Center.X + 20f, player.Center.Y, player.direction * 0.75f, 0, ground2, 0, 0f, player.whoAmI);
				}
			}
		}
	}
}
