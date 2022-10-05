using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetEaterofWorldsBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetEaterofWorldsHead>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetEaterofWorlds;

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetBool(player) = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[PetType] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				PetEaterofWorldsItem.Spawn(player, buffIndex: buffIndex);
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetEaterofWorldsBuff_AoMM : SimplePetBuffBase_AoMM<PetEaterofWorldsBuff>
	{

	}
}
