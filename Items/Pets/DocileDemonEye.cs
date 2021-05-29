using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileDemonEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Tooltip.SetDefault("Summons a docile Demon Eye to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.width = 34;
            Item.height = 22;
            Item.shoot = Mod.Find<ModProjectile>("DocileDemonEyeProj").Type;
            Item.buffType = Mod.Find<ModBuff>("DocileDemonEyeBuff").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            //regular recipe, dont delete
            CreateRecipe(1).AddIngredient(ItemID.BlackLens, 1).AddIngredient(ItemID.Lens, 1).AddTile(TileID.DemonAltar).Register();
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
