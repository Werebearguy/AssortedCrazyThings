using AssortedCrazyThings.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    [AutoloadEquip(EquipType.Balloon)]
    public class BipolarCandleInABalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bipolar Candle in a Balloon");
            Tooltip.SetDefault("Increased enemy spawn rate, reduced enemy spawn rate, and increased jump height");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.value = 0;
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<BipolarBuff>(), 60);
            player.jumpBoost = true;
            Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.7f, 0.7f, 0.7f);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<WaterCandleInABalloon>().AddIngredient<PeaceCandleInABalloon>().AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}
