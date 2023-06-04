using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NotAllDronePartsUnlockedCondition : Conditions.NeverTrue, IProvideItemConditionDescription
	{
		public static LocalizedText DescriptionText { get; private set; }

		public NotAllDronePartsUnlockedCondition()
		{
			string category = $"DropConditions.";
			DescriptionText ??= Language.GetOrRegister(ModContent.GetInstance<AssortedCrazyThings>().GetLocalizationKey($"{category}{GetType().Name}.Description"));
		}

		public new string GetConditionDescription()
		{
			return DescriptionText.ToString();
		}
	}
}
