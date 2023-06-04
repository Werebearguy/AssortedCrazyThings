using AssortedCrazyThings.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Weapons)]
	public class SlimePackMinionBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		private int SumOfSlimePackCounts(Player player)
		{
			int sum = player.ownedProjectileCounts[ModContent.ProjectileType<SlimePackMinion>()];
			sum += player.ownedProjectileCounts[ModContent.ProjectileType<SlimePackSpikedMinion>()];
			sum += player.ownedProjectileCounts[ModContent.ProjectileType<SlimePackAssortedMinion>()];
			return sum;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			AssPlayer modPlayer = player.GetModPlayer<AssPlayer>();
			if (SumOfSlimePackCounts(player) > 0)
			{
				modPlayer.slimePackMinion = true;
			}
			if (!modPlayer.slimePackMinion)
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
