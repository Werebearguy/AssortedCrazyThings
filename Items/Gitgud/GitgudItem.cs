using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
    /// <summary>
    /// Serves as a base for all gitgud items
    /// </summary>
    public abstract class GitgudItem : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = GitgudData.GetIndexFromItemType(item.type);
            int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
            if (insertIndex == -1) insertIndex = tooltips.Count;
            if (tooltips.FindIndex(l => l.Name == "Social") == -1)
            {
                if (index != -1)
                {
                    GitgudData data = GitgudData.DataList[index];
                    tooltips.Insert(insertIndex++, new TooltipLine(mod, "Desc", "Consolation Prize"));
                    string reduced = "" + (data.Reduction * 100) + "% reduced damage taken " + (data.Invasion != "" ? "during " + data.Invasion : "from " + data.BossName);
                    tooltips.Insert(insertIndex++, new TooltipLine(mod, "Reduced", reduced));
                    if (data.BuffType != -1)
                    {
                        tooltips.Insert(insertIndex++, new TooltipLine(mod, "BuffImmune", "Immunity to '" + data.BuffName + "' while " + data.BossName + (data.BossName.Contains(" or ") ? " are" : " is") + " alive"));
                    }

                    if (!(data.Accessory[Main.myPlayer] || Main.LocalPlayer.HasItem(item.type) || Main.LocalPlayer.trashItem.type == item.type))
                    {
                        tooltips.Insert(insertIndex++, new TooltipLine(mod, "Count", "Times died: " + data.Counter[Main.myPlayer] + "/" + data.CounterMax));
                    }
                }
                tooltips.Insert(insertIndex++, new TooltipLine(mod, "Gitgud", "[c/E180CE:'git gud']"));
            }
        }

        public sealed override void SetDefaults()
        {
            item.value = Item.sellPrice(copper: 1);
            item.rare = -1;
            item.maxStack = 1;
            item.accessory = true;

            //item.width = 32;
            //item.height = 32;

            MoreSetDefaults();
        }

        public virtual void MoreSetDefaults()
        {

        }
    }
}
