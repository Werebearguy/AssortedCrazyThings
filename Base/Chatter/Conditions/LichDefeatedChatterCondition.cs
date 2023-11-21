using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class LichDefeatedChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (ModContent.TryFind<ModNPC>("ThoriumMod/LichHeadless", out var modNPC1))
			{
				return MatchesNPCType(param, modNPC1.Type);
			}
			return false;
		}
	}
}
