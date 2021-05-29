using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class BabyCrimera : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Juicy Vertebrae");
            Tooltip.SetDefault("Summons a baby Crimera to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("BabyCrimera").Type;
            Item.buffType = Mod.Find<ModBuff>("BabyCrimera").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Vertebrae, 30).AddTile(TileID.DemonAltar).Register();
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
