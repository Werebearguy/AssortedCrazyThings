using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class DemonHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Heart");
            Tooltip.SetDefault("Summons a friendly Demon Heart to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("DemonHeart").Type;
            Item.buffType = Mod.Find<ModBuff>("DemonHeart").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.DemonHeart, 1).AddTile(TileID.DemonAltar).Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}
