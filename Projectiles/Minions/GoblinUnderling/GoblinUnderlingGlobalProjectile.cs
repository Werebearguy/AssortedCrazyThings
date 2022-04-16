using Terraria;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingGlobalProjectile : AssGlobalProjectile
	{
		public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
		{
			if (projectile.trap || projectile.npcProj)
			{
				return;
			}

			if (target.whoAmI != Main.myPlayer)
			{
				return;
			}

			if (damage < target.statLifeMax2 * 0.2f)
			{
				return;
			}

			foreach (var proj in GoblinUnderlingSystem.GetLocalGoblinUnderlings())
			{
				GoblinUnderlingSystem.TryCreate(proj, GoblinUnderlingMessageSource.PlayerHurt);
			}
		}
	}
}
