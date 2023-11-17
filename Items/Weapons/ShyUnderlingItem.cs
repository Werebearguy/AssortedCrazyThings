using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Shy;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class ShyUnderlingItem : GoblinUnderlingItem
	{
		public override int ProjType => ModContent.ProjectileType<ShyUnderlingProj>();

		public override int BuffType => ModContent.BuffType<ShyUnderlingBuff>();

		//TODO goblin value + obtainment
		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Ranged;
			Item.width = 18;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
		}
	}
}
