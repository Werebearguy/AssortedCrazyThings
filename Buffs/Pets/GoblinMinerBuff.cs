using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	public class GoblinMinerBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<GoblinMinerProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().GoblinMiner;

		public override void SafeSetStaticDefaults()
		{
			Main.vanityPet[Type] = false;
			Main.lightPet[Type] = true;
		}
	}
}
