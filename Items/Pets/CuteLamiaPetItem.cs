using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class CuteLamiaPetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<CuteLamiaPetProj>();

		public override int BuffType => ModContent.BuffType<CuteLamiaPetBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class CuteLamiaPetItem_AoMM : SimplePetItemBase_AoMM<CuteLamiaPetItem>
	{
		public override int BuffType => ModContent.BuffType<CuteLamiaPetBuff_AoMM>();
	}
}
