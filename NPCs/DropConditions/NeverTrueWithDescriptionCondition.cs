using Terraria.GameContent.ItemDropRules;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NeverTrueWithDescriptionCondition : Conditions.NeverTrue, IProvideItemConditionDescription
	{
		private readonly string description;
		public NeverTrueWithDescriptionCondition(string description) : base()
		{
			this.description = description;
		}

		public new string GetConditionDescription()
		{
			return description;
		}
	}
}
