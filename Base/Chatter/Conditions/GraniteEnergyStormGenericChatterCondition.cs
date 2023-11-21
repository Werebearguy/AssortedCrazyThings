using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class GraniteEnergyStormGenericChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (ModContent.TryFind<ModNPC>("ThoriumMod/GraniteEnergyStorm", out var modNPC))
			{
				return MatchesNPCType(param, modNPC.Type);
			}
			return false;
		}
	}
}
