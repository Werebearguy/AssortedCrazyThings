using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class QueenLarvaItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<QueenLarvaProj>();

		public override int BuffType => ModContent.BuffType<QueenLarvaBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 32;
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class QueenLarvaItem_AoMM : SimplePetItemBase_AoMM<QueenLarvaItem>
	{
		public override int BuffType => ModContent.BuffType<QueenLarvaBuff_AoMM>();
	}
}
