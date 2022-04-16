using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class FailureSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<FailureSlimeProj>();

		public override int BuffType => ModContent.BuffType<FailureSlimeBuff>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimy Skull");
			Tooltip.SetDefault("Summons a strange creature to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.rare = -11;
			Item.value = Item.sellPrice(silver: 40); //TODO value
		}

		//TODO obtain (grab bag?)
	}
}
