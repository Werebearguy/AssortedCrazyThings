using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetFishronItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soggy Fish Cake");
            Tooltip.SetDefault("Summons a friendly Fishron that flies with you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("PetFishronProj");
            item.buffType = mod.BuffType("PetFishronBuff");
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
