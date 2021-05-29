using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class VampireBat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fixed Bat Wing");
            Tooltip.SetDefault("Summons a friendly vampire that flies with you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("VampireBat").Type;
            Item.buffType = Mod.Find<ModBuff>("VampireBat").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 20);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BrokenBatWing, 2).AddIngredient(ItemID.SoulofNight, 10).AddTile(TileID.DemonAltar).Register();
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
