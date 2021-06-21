using System.Collections.Generic;
using Terraria;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace AssortedCrazyThings.Items
{
    public class DisabledGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            //Needs the item to be instantiated (ModItem assigned) before applying global
            return lateInstantiation && entity.ModItem is UnloadedItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is not UnloadedItem unloadedItem)
            {
                return;
            }

            FieldInfo modNameField = typeof(UnloadedItem).GetField("modName", BindingFlags.Instance | BindingFlags.NonPublic);
            if (modNameField == null)
            {
                return;
            }

            string modName = modNameField.GetValue(unloadedItem) as string;

            if (modName != Mod.Name)
            {
                return;
            }

            FieldInfo itemNameField = typeof(UnloadedItem).GetField("itemName", BindingFlags.Instance | BindingFlags.NonPublic);
            if (itemNameField == null)
            {
                return;
            }

            string itemName = itemNameField.GetValue(unloadedItem) as string;

            if (ConfigurationSystem.NonLoadedNames.Contains(itemName))
            {
                tooltips.Add(new TooltipLine(Mod, "UnloadedSource", "This item has been unloaded through the config"));
            }
        }
    }
}
