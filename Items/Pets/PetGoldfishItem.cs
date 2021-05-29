using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetGoldfishItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Fish Idol");
            Tooltip.SetDefault("Summons a goldfish that follows you"
                + "\n'You feel like you lost something important in obtaining this idol...'"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetGoldfishProj>();
            Item.buffType = ModContent.BuffType<PetGoldfishBuff>();
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
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
            CreateRecipe(1).AddIngredient(ItemID.FishStatue, 1).AddTile(TileID.DemonAltar).Register();
        }
    }
}
