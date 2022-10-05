using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class MeatballSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<MeatballSlimeProj>();

		public override int BuffType => ModContent.BuffType<MeatballSlimeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.HostileNPCs)]
	public class MeatballSlimeItem_AoMM : SimplePetItemBase_AoMM<MeatballSlimeItem>
	{
		public override int BuffType => ModContent.BuffType<MeatballSlimeBuff_AoMM>();
	}
}
