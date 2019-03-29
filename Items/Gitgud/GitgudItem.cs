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
            if (index != -1)
            {
                tooltips.Add(new TooltipLine(mod, "Tooltip0", "Consolation Prize"));
                tooltips.Add(new TooltipLine(mod, "Reduced", "" + (GitgudData.DataList[index].Reduction * 100) + "% reduced damage taken from " + GitgudData.DataList[index].BossName));
                if (GitgudData.DataList[index].BuffType != -1)
                {
                    tooltips.Add(new TooltipLine(mod, "BuffImmune", "Immunity to '" + GitgudData.DataList[index].BuffName + "' while " + GitgudData.DataList[index].BossName + " is alive"));
                }

                if (!GitgudData.DataList[index].Accessory[Main.myPlayer] && !Main.LocalPlayer.HasItem(item.type))
                {
                    tooltips.Add(new TooltipLine(mod, "Count", "Times died: " + GitgudData.DataList[index].Counter[Main.myPlayer] + "/" + GitgudData.DataList[index].CounterMax));
                }
            }
            tooltips.Add(new TooltipLine(mod, "Gitgud", "[c/E180CE:'git gud']"));
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
