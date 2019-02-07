using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileDemonEyeRed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unconscious Demon Eye");
            Tooltip.SetDefault("Summons a docile red Demon Eye to follow you"
                + "\nLegacy Appearance, craft into 'Docile Demon Eye' at your nearest Demon Altar"
                + "\nThis version of the pet will be discontinued in the next update");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("DocileDemonEyeRed");
            item.buffType = mod.BuffType("DocileDemonEyeRed");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
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
