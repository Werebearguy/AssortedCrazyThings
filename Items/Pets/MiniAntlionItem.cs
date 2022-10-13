using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class MiniAntlionItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<MiniAntlionProj>();

		public override int BuffType => ModContent.BuffType<MiniAntlionBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(silver: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.DroppedPets)]
	public class MiniAntlionItem_AoMM : SimplePetItemBase_AoMM<MiniAntlionItem>
	{
		public override int BuffType => ModContent.BuffType<MiniAntlionBuff_AoMM>();
	}
}
