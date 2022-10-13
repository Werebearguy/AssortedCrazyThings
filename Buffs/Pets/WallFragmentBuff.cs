using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class WallFragmentBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<WallFragmentMouth>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().WallFragment;

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetBool(player) = true;
			if (player.whoAmI == Main.myPlayer)
			{
				var source = player.GetSource_Buff(buffIndex);

				int eye1 = ModContent.ProjectileType<WallFragmentEye1>();
				if (player.ownedProjectileCounts[eye1] <= 0)
				{
					Projectile.NewProjectile(source, player.Center.X, player.Top.Y - 6f, player.direction * 0.75f, -0.5f, eye1, 0, 0f, player.whoAmI, 0f, 0f);
				}

				int mouth = ModContent.ProjectileType<WallFragmentMouth>();
				if (player.ownedProjectileCounts[mouth] <= 0)
				{
					Projectile.NewProjectile(source, player.Center.X, player.Center.Y, player.direction, 0f, mouth, 0, 0f, player.whoAmI, 0f, 0f);

				}

				int eye2 = ModContent.ProjectileType<WallFragmentEye2>();
				if (player.ownedProjectileCounts[eye2] <= 0)
				{
					Projectile.NewProjectile(source, player.Center.X, player.Bottom.Y + 6f, player.direction * 0.75f, 0.5f, eye2, 0, 0f, player.whoAmI, 0f, 0f);
				}
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class WallFragmentBuff_AoMM : SimplePetBuffBase_AoMM<WallFragmentBuff>
	{

	}
}
