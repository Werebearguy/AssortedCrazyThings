using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class IlluminantSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<IlluminantSlimeProj>();

		public override int BuffType => ModContent.BuffType<IlluminantSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class IlluminantSlimeItem_AoMM : SimplePetItemBase_AoMM<IlluminantSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<IlluminantSlimeBuff_AoMM>();
	}
}
