using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class SunPetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun in a bottle");
            Tooltip.SetDefault("Summons a small sun that provides you with constant sunlight");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("SunPetProj");
            item.buffType = mod.BuffType("SunPetBuff");
            item.width = 20;
            item.height = 26;
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
