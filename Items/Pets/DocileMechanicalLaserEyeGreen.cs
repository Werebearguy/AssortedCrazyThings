using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class DocileMechanicalLaserEyeGreen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unconscious Laser Eye");
            Tooltip.SetDefault("Summons a docile green Laser Eye to follow you."
                + "\nLegacy Appearance, use 'Docile Demon Eye' instead."
                + "\nThis version of the pet will be discontinued in the next update.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("DocileMechanicalLaserEyeGreen");
            item.buffType = mod.BuffType("DocileMechanicalLaserEyeGreen");
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
