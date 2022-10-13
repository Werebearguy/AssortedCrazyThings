using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class LilWrapsItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<LilWrapsProj>();

		public override int BuffType => ModContent.BuffType<LilWrapsBuff>();

		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class LilWrapsItem_AoMM : SimplePetItemBase_AoMM<LilWrapsItem>
	{
		public override int BuffType => ModContent.BuffType<LilWrapsBuff_AoMM>();
	}
}
