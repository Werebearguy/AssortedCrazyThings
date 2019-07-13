using AssortedCrazyThings.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
    class EnhancedHunterPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enhanced Hunter Potion");
            Tooltip.SetDefault("Shows the location of enemies"
                + "\nAdditionally, shows the location of enemies outside your vision range"
                + "\nRange is roughly one screen in each direction");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.buffTime = 18000; //five minutes
            item.buffType = mod.BuffType<EnhancedHunterBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(silver: 3);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HunterPotion, 1);
            recipe.AddIngredient(ItemID.PixieDust, 1);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
