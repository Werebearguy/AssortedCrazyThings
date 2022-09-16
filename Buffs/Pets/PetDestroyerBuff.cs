using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetDestroyerBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetDestroyerHead>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetDestroyer;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Destroyer");
			Description.SetDefault("A tiny Destroyer and two tiny Probes are following you");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetBool(player) = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[PetType] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				PetDestroyerItem.Spawn(player, buffIndex: buffIndex);
			}
		}
	}
}
