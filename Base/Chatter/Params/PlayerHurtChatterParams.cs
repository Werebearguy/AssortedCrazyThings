using Terraria;

namespace AssortedCrazyThings.Base.Chatter
{
	public class PlayerHurtChatterParams : IChatterParams
	{
		public Entity Attacker { get; init; }
		public Player.HurtInfo HurtInfo { get; init; }

		public PlayerHurtChatterParams(Entity attacker, Player.HurtInfo hurtInfo)
		{
			Attacker = attacker;
			HurtInfo = hurtInfo;
		}
	}
}
