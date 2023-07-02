using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Weapons)]
	public class EagerUnderlingBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			EagerUnderlingPlayer modPlayer = player.GetModPlayer<EagerUnderlingPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EagerUnderlingProj>()] > 0)
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
