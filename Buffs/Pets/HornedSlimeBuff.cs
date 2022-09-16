using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class HornedSlimeBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<HornedSlimeProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().HornedSlime;

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Horned Slime");
			Description.SetDefault("A Horned Slime is following you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}
	}
}
