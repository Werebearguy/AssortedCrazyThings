using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	[Content(ContentType.DroppedPets)]
	public class WobyBuff : SimplePetBuffBase
	{
		public override int PetType => ModContent.ProjectileType<WobyProj>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Woby;
	}

	//Deviaton from the norm: has custom buff and projectile
	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class WobyBuff_AoMM : SimplePetBuffBase
	{
		public static ModBuff BaseModBuff => BuffLoader.GetBuff(ModContent.BuffType<WobyBuff>());

		public override LocalizedText DisplayName => AmuletOfManyMinionsApi.AppendAoMMVersion(BaseModBuff.DisplayName);

		public override LocalizedText Description => BaseModBuff.Description;

		public override int PetType => ModContent.ProjectileType<WobyProj_AoMM>();

		public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().Woby_AoMM;
	}
}
