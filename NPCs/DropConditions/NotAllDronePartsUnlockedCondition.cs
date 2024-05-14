using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NotAllDronePartsUnlockedCondition : IItemDropRuleCondition, IProvideItemConditionDescription
	{
		public static LocalizedText DescriptionText { get; private set; }

		public NotAllDronePartsUnlockedCondition()
		{
			DescriptionText ??= AssUtils.GetDropConditionDescription(GetType().Name);
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsInSimulation)
			{
				return false;
			}

			return !DroneController.AllUnlocked(info.player);
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return DescriptionText.ToString();
		}
	}
}
