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
            Item.width = 20;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.buffTime = 18000; //five minutes
            Item.buffType = ModContent.BuffType<EnhancedHunterBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 3);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HunterPotion, 1).AddIngredient(ItemID.PixieDust, 1).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 1).AddTile(TileID.Bottles).Register();
        }
    }
}
