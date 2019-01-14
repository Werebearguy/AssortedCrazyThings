using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfPainSuppression : ModItem
    {

        //TODO set the time in seconds that the item stays in cooldown in AssPlayer.cs, here: GetDefenseTimerMax
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Pain Suppression");
            Tooltip.SetDefault("Drastically increases your defense when you are at critically low health.");
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronskinPotion, 5);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulAwakened>(), 10);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
