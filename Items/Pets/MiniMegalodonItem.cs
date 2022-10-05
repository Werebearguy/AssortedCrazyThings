using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
	[LegacyName("MiniMegalodon")]
	public class MiniMegalodonItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<MiniMegalodonProj>();

		public override int BuffType => ModContent.BuffType<MiniMegalodonBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs | ContentType.DroppedPets)]
	public class MiniMegalodonItem_AoMM : SimplePetItemBase_AoMM<MiniMegalodonItem>
	{
		public override int BuffType => ModContent.BuffType<MiniMegalodonBuff_AoMM>();
	}
}
