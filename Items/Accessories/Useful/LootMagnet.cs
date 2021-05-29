using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    public class LootMagnet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot Magnet");
            Tooltip.SetDefault("Increased coin pickup range"
                + "\nIncreased heart and mana pickup range"
                + "\nHitting enemies will sometimes Drop extra coins"
                + "\nShops have lowered prices");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaMagnet = true;                       //This is the effect of the Celestial Magnet.
            player.lifeMagnet = true;                       //This is the effect of the Heartreach Potion.
            player.goldRing = true;                         //This is the effect of the Gold Ring.
            player.discount = true;                         //This is the effect of the Discount Card.
            player.hasLuckyCoin = true;						//This is the effect of the Lucky Coin.
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.GreedyRing, 1).AddIngredient(ItemID.CelestialMagnet, 1).AddIngredient(ItemID.HeartreachPotion, 1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
