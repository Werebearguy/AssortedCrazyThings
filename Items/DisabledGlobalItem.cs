using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace AssortedCrazyThings.Items
{
    //Responsible for showing why an item was unloaded/disabled
    [Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
    public class DisabledGlobalItem : AssGlobalItem
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

            if (unloadedItem.ModName != Mod.Name)
            {
                return;
            }

            if (ConfigurationSystem.NonLoadedNames.TryGetValue(unloadedItem.ItemName, out ContentType type))
            {
                tooltips.Add(new TooltipLine(Mod, "UnloadedSource", $"Disabled by the '{ConfigurationSystem.ContentTypeToString(type)}' config setting"));
            }
        }
    }
}
