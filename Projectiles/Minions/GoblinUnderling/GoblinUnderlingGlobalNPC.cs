using Terraria;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingGlobalNPC : AssGlobalNPC
	{
        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
		{
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
