using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfPainSuppression : ModItem //todo proper name
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Pain Suppression"); //todo proper name
            Tooltip.SetDefault("Drastically increases your defense when you are at critically low health");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 20, 0, 0);
            item.rare = -11;
            item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);
            if (mPlayer.canGetDefense)
            {
                //the first string is irrelevant, its never used anywhere, basically just a name for that line
                tooltips.Add(new TooltipLine(mod, "CanUse", "Ready to use"));
            }
            else
            {
                string timeName;
                if(mPlayer.getDefenseTimer > 60) //more than 1 minute
                {
                    if (mPlayer.getDefenseTimer > 90) //more than 1:30 minutes because of round
                    {
                        timeName = " minutes";
                    }
                    else
                    {
                        timeName = " minute";
                    }
                    tooltips.Add(new TooltipLine(mod, "UsableIn", "Ready again in " + Math.Round(mPlayer.getDefenseTimer / 60f) + timeName));
                }
                else
                {
                    if (mPlayer.getDefenseTimer > 1) //more than 1 second
                    {
                        timeName = " seconds";
                    }
                    else
                    {
                        timeName = " second";
                    }
                    tooltips.Add(new TooltipLine(mod, "UsableIn", "Ready again in " + mPlayer.getDefenseTimer + timeName));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().getDefense = true;
        }

        //TODO recipe
        //TODO set the time in seconds that the item stays in cooldown in AssPlayer.cs, here:
        // private const short GetDefenseTimerTimerMax = 1200; == 20 minutes
    }
}
