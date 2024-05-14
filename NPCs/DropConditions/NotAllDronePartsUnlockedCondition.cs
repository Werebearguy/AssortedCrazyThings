using AssortedCrazyThings.Items.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DropConditions
{
	public class NotAllDronePartsUnlockedCondition : IItemDropRuleCondition
	{
		public static LocalizedText DescriptionText { get; private set; }

		public NotAllDronePartsUnlockedCondition()
		{
			string category = $"DropConditions.";
			DescriptionText ??= Language.GetOrRegister(ModContent.GetInstance<AssortedCrazyThings>().GetLocalizationKey($"{category}{GetType().Name}.Description"));
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
