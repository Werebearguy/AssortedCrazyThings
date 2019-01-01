using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfRetreat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Retreat");
            Tooltip.SetDefault("Teleports you home when health is dangerously low");
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
            if (mPlayer.canTeleportHomeWhenLow)
            {
                //the first string is irrelevant, its never used anywhere, basically just a name for that line
                tooltips.Add(new TooltipLine(mod, "CanUse", "Ready to use"));
            }
            else
            {
                string timeName;
                if(mPlayer.teleportHomeWhenLowTimer > 60) //more than 1 minute
                {
                    if (mPlayer.teleportHomeWhenLowTimer > 90) //more than 1:30 minutes because of round
                    {
                        timeName = " minutes";
                    }
                    else
                    {
                        timeName = " minute";
                    }
                    tooltips.Add(new TooltipLine(mod, "UsableIn", "Ready again in " + Math.Round(mPlayer.teleportHomeWhenLowTimer/60f) + timeName));
                }
                else
                {
                    if (mPlayer.teleportHomeWhenLowTimer > 1) //more than 1 second
                    {
                        timeName = " seconds";
                    }
                    else
                    {
                        timeName = " second";
                    }
                    tooltips.Add(new TooltipLine(mod, "UsableIn", "Ready again in " + mPlayer.teleportHomeWhenLowTimer + timeName));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().teleportHomeWhenLow = true;
        }

        //TODO recipe
        //TODO set the time in seconds that the item stays in cooldown in AssPlayer.cs, here:
        // private const short TeleportHomeWhenLowTimerMax = 1200; == 20 minutes
    }
}
