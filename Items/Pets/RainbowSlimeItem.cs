using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class RainbowSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<RainbowSlimeProj>();

		public override int BuffType => ModContent.BuffType<RainbowSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class RainbowSlimeItem_AoMM : SimplePetItemBase_AoMM<RainbowSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<RainbowSlimeBuff_AoMM>();
	}
}
