using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class PigronataItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pigronata");
            Tooltip.SetDefault("Summons a friendly Pigronata to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("Pigronata").Type;
            Item.buffType = Mod.Find<ModBuff>("PigronataBuff").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 2, silver: 20);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Pigronata, 1).AddIngredient(ItemID.LifeFruit, 1).AddTile(TileID.DemonAltar).Register();
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
