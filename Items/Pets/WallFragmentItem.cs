using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class WallFragmentItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<WallFragmentMouth>();

		public override int BuffType => ModContent.BuffType<WallFragmentBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 26;
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class WallFragmentItem_AoMM : SimplePetItemBase_AoMM<WallFragmentItem>
	{
		public override int BuffType => ModContent.BuffType<WallFragmentBuff_AoMM>();
	}
}
