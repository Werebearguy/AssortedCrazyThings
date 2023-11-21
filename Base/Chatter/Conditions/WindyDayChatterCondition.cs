using Terraria;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	public class WindyDayChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return Main.LocalPlayer.position.Y / 16 < Main.worldSurface && NPC.TooWindyForButterflies; //Ladybugs spawn when this is true
		}
	}
}
