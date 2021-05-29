using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class LifelikeMechanicalFrog : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lifelike Mechanical Frog");
            Tooltip.SetDefault("Summons a friendly Frog to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("LifelikeMechanicalFrog").Type;
            Item.buffType = Mod.Find<ModBuff>("LifelikeMechanicalFrog").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Frog, 1).AddTile(TileID.Anvils).Register();
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
