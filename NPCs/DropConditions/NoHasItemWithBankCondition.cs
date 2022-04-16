using AssortedCrazyThings.Base;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NoHasItemWithBankCondition : IItemDropRuleCondition, IProvideItemConditionDescription
	{
		private int itemType;

		public NoHasItemWithBankCondition(int itemType)
        {
			this.itemType = itemType;
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

		public string GetConditionDescription() => $"Only if {Lang.GetItemNameValue(itemType)} is not in the closest players inventory or banks";
	}
}
