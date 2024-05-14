using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class MatchAppearanceCondition : Conditions.IsUsingSpecificAIValues, IProvideItemConditionDescription
	{
		public static LocalizedText DescriptionText { get; set; }

		public MatchAppearanceCondition(int aiSlot, int valueToMatch) : base(aiSlot, valueToMatch)
		{
			//Loaded earlier as it's used elsewhere
		}

		public new bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation)
			{
				return false;
			}

			return base.CanDrop(info);
		}

		public new string GetConditionDescription()
		{
			return DescriptionText.ToString();
		}
	}
}
