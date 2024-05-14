using AssortedCrazyThings.Base;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NoHasItemWithBankCondition : IItemDropRuleCondition, IProvideItemConditionDescription
	{
		public static LocalizedText DescriptionText { get; private set; }

		private int itemType;

		public NoHasItemWithBankCondition(int itemType)
		{
			this.itemType = itemType;

			DescriptionText ??= AssUtils.GetDropConditionDescription(GetType().Name);
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation)
			{
				return false;
			}

			return !info.player.HasItemWithBanks(itemType);
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return DescriptionText.Format(Lang.GetItemNameValue(itemType));
		}
	}
}
