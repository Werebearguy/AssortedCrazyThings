using AssortedCrazyThings.Projectiles.Minions.GoblinUnderling;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Goblin Underling");
			// Description.SetDefault("A loyal goblin underling is fighting for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			GoblinUnderlingPlayer modPlayer = player.GetModPlayer<GoblinUnderlingPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<GoblinUnderlingProj>()] > 0)
			{
				modPlayer.hasMinion = true;
			}
			if (!modPlayer.hasMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
