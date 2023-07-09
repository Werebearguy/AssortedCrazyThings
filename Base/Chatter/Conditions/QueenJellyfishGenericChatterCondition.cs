using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class QueenJellyfishGenericChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (ModContent.TryFind<ModNPC>("ThoriumMod/QueenJellyfish", out var modNPC))
			{
				return MatchesNPCType(param, modNPC.Type);
			}
			return false;
		}
	}
}
