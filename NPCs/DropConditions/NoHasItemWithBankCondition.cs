using AssortedCrazyThings.Base;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NoHasItemWithBankCondition : IItemDropRuleCondition, IProvideItemConditionDescription
	{
		public static LocalizedText DescriptionText { get; private set; }

		private int itemType;

		public NoHasItemWithBankCondition(int itemType)
		{
			this.itemType = itemType;

			string category = $"DropConditions.";
			DescriptionText ??= Language.GetOrRegister(ModContent.GetInstance<AssortedCrazyThings>().GetLocalizationKey($"{category}{GetType().Name}.Description"));
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation)
			{
				return false;
			}

			return !info.player.HasItemWithBanks(itemType);
		}

		public bool CanShowItemDropInUI() => true;

		public string GetConditionDescription() => DescriptionText.Format(Lang.GetItemNameValue(itemType));
	}
}
