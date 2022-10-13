using AssortedCrazyThings.Items.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class PetPlanteraBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<PetPlanteraProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetPlantera;

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			PetBool(player) = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[PetType] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				PetPlanteraItem.Spawn(player, buffIndex: buffIndex);
			}
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class PetPlanteraBuff_AoMM : SimplePetBuffBase_AoMM<PetPlanteraBuff>
	{

	}
}
