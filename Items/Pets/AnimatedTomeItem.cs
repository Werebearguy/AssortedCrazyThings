using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.FriendlyNPCs)]
	public class AnimatedTomeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<AnimatedTomeProj>();

		public override int BuffType => ModContent.BuffType<AnimatedTomeBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}

	[Content(ContentType.AommSupport | ContentType.FriendlyNPCs)]
	public class AnimatedTomeItem_AoMM : SimplePetItemBase_AoMM<AnimatedTomeItem>
	{
		public override int BuffType => ModContent.BuffType<AnimatedTomeBuff_AoMM>();
	}
}
