using Terraria.GameContent.ItemDropRules;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class MatchAppearanceCondition : Conditions.IsUsingSpecificAIValues, IProvideItemConditionDescription
	{
		public MatchAppearanceCondition(int aiSlot, int valueToMatch) : base(aiSlot, valueToMatch)
		{

		}

		public new bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return base.CanDrop(info);
			}
			return false;
		}

		public new string GetConditionDescription()
		{
			return "Drops based on appearance";
		}
	}
}
