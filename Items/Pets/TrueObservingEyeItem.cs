using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets)]
	public class TrueObservingEyeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TrueObservingEyeProj>();

		public override int BuffType => ModContent.BuffType<TrueObservingEyeBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Observing Eye");
			Tooltip.SetDefault("Summons a True Eye of Cthulhu to watch after you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
