using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TinyTwinsBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<TinySpazmatismProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TinyTwins;

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetBool(player) = true;
			if (player.whoAmI == Main.myPlayer)
			{
				var source = player.GetSource_Buff(buffIndex);

				int spaz = ModContent.ProjectileType<TinySpazmatismProj>();
				if (player.ownedProjectileCounts[spaz] <= 0)
				{
					Projectile.NewProjectile(source, player.position.X + (player.width / 2), player.position.Y + (player.height / 2), -player.direction, 0f, spaz, 0, 0f, player.whoAmI);
				}

				int reti = ModContent.ProjectileType<TinyRetinazerProj>();
				if (player.ownedProjectileCounts[reti] <= 0)
				{
					Projectile.NewProjectile(source, player.position.X + (player.width / 2), player.position.Y, -player.direction, 0f, reti, 0, 0f, player.whoAmI);
				}
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class TinyTwinsBuff_AoMM : SimplePetBuffBase_AoMM<TinyTwinsBuff>
	{

	}
}
