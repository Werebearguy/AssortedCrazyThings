using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class ViscountGenericChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (ModContent.TryFind<ModNPC>("ThoriumMod/Viscount", out var modNPC))
			{
				return MatchesNPCType(param, modNPC.Type);
			}
			return false;
		}
	}
}
