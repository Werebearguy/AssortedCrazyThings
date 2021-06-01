using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class StingSlimeBlackItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<StingSlimeBlackProj>();

        public override int BuffType => ModContent.BuffType<StingSlimeBlackBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Sting Slime");
            Tooltip.SetDefault("Summons a friendly black Sting Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
