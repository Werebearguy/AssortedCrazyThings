using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class MeatballItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<MeatballSlimeProj>();

        public override int BuffType => ModContent.BuffType<MeatballSlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Meatball");
            Tooltip.SetDefault("Summons Meatball to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
