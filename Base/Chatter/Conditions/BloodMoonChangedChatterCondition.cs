using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class BloodMoonChangedChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			if (param is not BloodMoonChangedChatterParams p)
			{
				return false;
			}

			return p.Current == Main.bloodMoon && p.Current != p.Prev;
		}
	}
}
