using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.HostileNPCs)]
	public class TurtleSlimeItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TurtleSlimeProj>();

		public override int BuffType => ModContent.BuffType<TurtleSlimeBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Turtle Slime");
			Tooltip.SetDefault("Summons a friendly Turtle Slime to follow you");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(copper: 10);
		}
	}
}
