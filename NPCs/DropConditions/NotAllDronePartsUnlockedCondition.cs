using AssortedCrazyThings.Items.DroneUnlockables;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.DropConditions
{
    public class NotAllDronePartsUnlockedCondition : Conditions.NeverTrue, IProvideItemConditionDescription
    {
        public new string GetConditionDescription()
        {
            return $"Drops if not all {Lang.GetItemNameValue(ModContent.ItemType<DroneParts>())} are unlocked yet";
        }
    }
}
