using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfRetreat : ModItem
    {

        //TODO set the time in seconds that the item stays in cooldown in AssPlayer.cs, here: TeleportHomeTimerMax
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Retreat");
            Tooltip.SetDefault("Teleports you home when health is dangerously low"
                + "\nHas a cooldown of " + (AssPlayer.TeleportHomeTimerMax / 60) + " minutes");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = -11;
            item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);
            if (mPlayer.canTeleportHome)
            {
                tooltips.Add(new TooltipLine(mod, "Ready", "Ready to use"));
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
                if (mPlayer.teleportHomeTimer > 60) //more than 1 minute
                {
                    if (mPlayer.teleportHomeTimer > 90) //more than 1:30 minutes because of round
                    {
                        timeName = " minutes";
                    }
                    else
                    {
                        timeName = " minute";
                    }
                    tooltips.Add(new TooltipLine(mod, "Ready", "Ready again in " + Math.Round(mPlayer.teleportHomeTimer / 60f) + timeName + dots));
                }
                else
                {
                    if (mPlayer.teleportHomeTimer > 1) //more than 1 second
                    {
                        timeName = " seconds";
                    }
                    else
                    {
                        timeName = " second";
                    }
                    tooltips.Add(new TooltipLine(mod, "Ready", "Ready again in " + mPlayer.teleportHomeTimer + timeName + dots));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AssPlayer>().teleportHome = true;
        }
    }
}
