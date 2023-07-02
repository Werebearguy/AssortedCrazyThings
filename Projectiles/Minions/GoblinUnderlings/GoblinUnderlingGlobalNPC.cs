using Terraria;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingGlobalNPC : AssGlobalNPC
	{
		public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
		{
			if (target.whoAmI != Main.myPlayer)
			{
				return;
			}

			if (hurtInfo.Damage < target.statLifeMax2 * 0.2f)
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
