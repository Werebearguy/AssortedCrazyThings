using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Gitgud
{
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
                    tooltips.Insert(insertIndex++, new TooltipLine(mod, "Desc", "Consolation Prize"));
                    string reduced = "" + (GitgudData.DataList[index].Reduction * 100) + "% reduced damage taken " + (GitgudData.DataList[index].Invasion != "" ? "during " + GitgudData.DataList[index].Invasion : "from " + GitgudData.DataList[index].BossName);
                    tooltips.Insert(insertIndex++, new TooltipLine(mod, "Reduced", reduced));
                    if (GitgudData.DataList[index].BuffType != -1)
                    {
                        tooltips.Insert(insertIndex++, new TooltipLine(mod, "BuffImmune", "Immunity to '" + GitgudData.DataList[index].BuffName + "' while " + GitgudData.DataList[index].BossName + " is alive"));
                    }

                    if (!(GitgudData.DataList[index].Accessory[Main.myPlayer] || Main.LocalPlayer.HasItem(item.type) || Main.LocalPlayer.trashItem.type == item.type))
                    {
                        tooltips.Insert(insertIndex++, new TooltipLine(mod, "Count", "Times died: " + GitgudData.DataList[index].Counter[Main.myPlayer] + "/" + GitgudData.DataList[index].CounterMax));
                    }
                }
                tooltips.Insert(insertIndex++, new TooltipLine(mod, "Gitgud", "[c/E180CE:'git gud']"));
            }
        }

        public sealed override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
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
