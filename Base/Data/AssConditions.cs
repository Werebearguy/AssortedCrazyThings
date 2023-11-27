using AssortedCrazyThings.Items.Weapons;
using Terraria;

namespace AssortedCrazyThings.Base.Data
{
	public static class AssConditions
	{
		public static readonly string CommonKey = "Conditions.";

		public static readonly Condition UnlockedBiomeTorches = new Condition(AssUtils.Instance.GetLocalization($"{CommonKey}UnlockedBiomeTorches"), () => Main.LocalPlayer.unlockedBiomeTorches);

		public static Condition DroneTypeNotUnlocked(DroneType droneType)
		{
			return new Condition(AssUtils.Instance.GetLocalization($"{CommonKey}DroneTypeNotUnlocked"), () => !Main.LocalPlayer.GetModPlayer<AssPlayer>().droneControllerUnlocked.HasFlag(droneType));
		}
	}
}
