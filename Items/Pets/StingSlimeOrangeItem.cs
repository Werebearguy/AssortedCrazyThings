using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class StingSlimeOrangeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<StingSlimeOrangeProj>();

        public override int BuffType => ModContent.BuffType<StingSlimeOrangeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Sting Slime");
            Tooltip.SetDefault("Summons a friendly orange Sting Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
