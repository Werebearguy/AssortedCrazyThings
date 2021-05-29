using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class AbeeminationItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abeemination");
            Tooltip.SetDefault("Summons a friendly Abeemination to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<AbeeminationProj>();
            Item.buffType = ModContent.BuffType<AbeeminationBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Abeemination, 1).AddIngredient(ItemID.LifeFruit, 1).AddTile(TileID.DemonAltar).Register();
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
