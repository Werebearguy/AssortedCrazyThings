using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[LegacyName("BrainofConfusion")]
	public class BrainofConfusionItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<BrainofConfusionProj>();

		public override int BuffType => ModContent.BuffType<BrainofConfusionBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brain of Confusion");
			Tooltip.SetDefault("Summons a Brain of Confusion to follow aimlessly behind you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
		}
	}
}
