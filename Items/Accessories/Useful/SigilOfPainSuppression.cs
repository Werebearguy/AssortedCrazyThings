using AssortedCrazyThings.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class SigilOfPainSuppression : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Pain Suppression");
            Tooltip.SetDefault("Drastically increases your defense when you are at critically low health"
                + "\nHas a cooldown of " + (AssPlayer.GetDefenseTimerMax / 60) + " minutes");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 22;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = -11;
            item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);

            bool inVanitySlot = false;

            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Name == "SocialDesc")
                {
                    inVanitySlot = true;
                    tooltips[i].text = "Cooldown will go down while in social slot";
                    break;
                }
            }

            int insertIndex = tooltips.FindLastIndex(l => l.Name.StartsWith("Tooltip"));
            if (insertIndex == -1) insertIndex = tooltips.Count;

            if (!inVanitySlot)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Name == "Tooltip1")
                    {
                        insertIndex = i + 1; //it inserts "left" of where it found the index (without +1), so everything else get pushed one up
                        break;
                    }
                }
            }

            if (AssUtils.ItemInInventoryOrEquipped(Main.LocalPlayer, item))
            {
                if (mPlayer.canGetDefense)
                {
                    tooltips.Insert(insertIndex, new TooltipLine(mod, "Ready", "Ready to use"));
                }
                else
                {
                    //create animating "..." effect after the Ready line
                    string dots = "";
                    int dotCount = ((int)Main.time % 120) / 30; //from 0 to 30, from 31 to 60, from 61 to 90

                    for (int i = 0; i < dotCount; i++)
                    {
                        dots += ".";
                    }

                    string timeName;
                    if (mPlayer.getDefenseTimer > 60) //more than 1 minute
                    {
                        if (mPlayer.getDefenseTimer > 90) //more than 1:30 minutes because of round
                        {
                            timeName = " minutes";
                        }
                        else
                        {
                            timeName = " minute";
                        }
                        tooltips.Insert(insertIndex, new TooltipLine(mod, "Ready", "Ready again in " + Math.Round(mPlayer.getDefenseTimer / 60f) + timeName + dots));
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
                        tooltips.Insert(insertIndex, new TooltipLine(mod, "Ready", "Ready again in " + mPlayer.getDefenseTimer + timeName + dots));
                    }
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().getDefense = true;
        }
    }
}
