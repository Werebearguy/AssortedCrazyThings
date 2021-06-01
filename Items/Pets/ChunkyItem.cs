using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class ChunkyItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<ChunkySlimeProj>();

        public override int BuffType => ModContent.BuffType<ChunkySlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Chunky");
            Tooltip.SetDefault("Summons Chunky to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
