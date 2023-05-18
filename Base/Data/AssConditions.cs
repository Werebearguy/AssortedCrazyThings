using AssortedCrazyThings.Items.Weapons;
using Terraria;
using Terraria.Localization;

namespace AssortedCrazyThings.Base.Data
{
	public static class AssConditions
	{
		public static readonly string CommonKey = "Conditions.";

		public static readonly Condition UnlockedBiomeTorches = new Condition(Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{CommonKey}UnlockedBiomeTorches")), () => Main.LocalPlayer.unlockedBiomeTorches);

		public static readonly Condition PlaceablesFunctional = new Condition(Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{CommonKey}PlaceablesFunctional")), () => ContentConfig.Instance.PlaceablesFunctional);

		public static readonly Condition VanityAccessories = new Condition(Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{CommonKey}VanityAccessories")), () => ContentConfig.Instance.VanityAccessories);

		public static Condition DroneTypeNotUnlocked(DroneType droneType)
		{
			return new Condition(Language.GetOrRegister(AssUtils.Instance.GetLocalizationKey($"{CommonKey}DroneTypeNotUnlocked")), () => Main.LocalPlayer.GetModPlayer<AssPlayer>().droneControllerUnlocked.HasFlag(droneType));
		}
	}
}
