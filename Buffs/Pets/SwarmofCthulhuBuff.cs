using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class SwarmofCthulhuBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<SwarmofCthulhuProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SwarmofCthulhu;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Swarm of Cthulhu");
			Description.SetDefault("A Swarm of Cthulhu is following you");
		}
	}
}
