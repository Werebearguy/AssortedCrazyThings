using System.Collections.Generic;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public abstract class MinionItemBase : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //need a dummy because you can't remove elements from a list while you are iterating
            TooltipLine line = new TooltipLine(mod, "dummy", "dummy");
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "BuffTime")
                {
                    line = line2;
                }
            }
            if (line.Name != "dummy") tooltips.Remove(line);

            MoreModifyTooltips(tooltips);
        }

        public virtual void MoreModifyTooltips(List<TooltipLine> tooltips)
        {

        }
    }

}
