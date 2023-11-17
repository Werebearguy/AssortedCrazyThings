using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Serious;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class SeriousUnderlingItem : GoblinUnderlingItem
	{
		public override int ProjType => ModContent.ProjectileType<SeriousUnderlingProj>();

		public override int BuffType => ModContent.BuffType<SeriousUnderlingBuff>();

		//TODO goblin value + obtainment
		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Magic;
			Item.width = 26;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
		}
	}
}
