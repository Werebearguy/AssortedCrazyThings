using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NeverTrueWithDescriptionCondition : Conditions.NeverTrue, IProvideItemConditionDescription
	{
		private LocalizedText DescriptionText { get; init; }

		public NeverTrueWithDescriptionCondition(LocalizedText descriptionText) : base()
		{
			DescriptionText = descriptionText;
		}

		public new string GetConditionDescription()
		{
			return DescriptionText.ToString();
		}
	}
}
