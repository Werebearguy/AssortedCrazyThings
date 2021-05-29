using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetMoonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Moon");
            Tooltip.SetDefault("Summons a small moon that provides you with constant moonlight"
                + "\nShows the current moon cycle in the buff tip"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetMoonProj>();
            Item.buffType = ModContent.BuffType<PetMoonBuff>();
            Item.width = 20;
            Item.height = 26;
            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 7);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Bottle).AddIngredient(ItemID.MoonStone).AddIngredient(ItemID.Sextant).AddTile(TileID.CrystalBall).Register();
        }
    }
}
