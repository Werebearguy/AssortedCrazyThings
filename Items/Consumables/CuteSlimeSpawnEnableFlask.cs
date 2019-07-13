using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
    public class CuteSlimeSpawnEnableFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellied Ale");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (AssUtils.AssConfig.CuteSlimesPotionOnly)
            {
                tooltips.Add(new TooltipLine(mod, "Tooltip", "Allows you to see Cute Slimes for a short time"));
            }
            else
            {
                tooltips.Add(new TooltipLine(mod, "Tooltip", "You will see Cute Slimes more often for a short time"));
            }
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 20;
            item.height = 28;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.buffTime = 18000; //five minutes
            item.buffType = mod.BuffType<CuteSlimeSpawnEnableBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 20);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ale, 1);
            recipe.AddIngredient(ItemID.Gel, 1);
            recipe.AddTile(TileID.Kegs);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
