using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[LegacyName("GoblinUnderlingItem")]
	public class EagerUnderlingItem : GoblinUnderlingItem
	{
		public override int ProjType => ModContent.ProjectileType<EagerUnderlingProj>();

		public override int BuffType => ModContent.BuffType<EagerUnderlingBuff>();

		public override void SafeSetDefaults()
		{
			currentClass = GoblinUnderlingClass.Melee;
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
		}
	}
}
