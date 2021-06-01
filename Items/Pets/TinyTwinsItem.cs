using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class TinyTwinsItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<TinySpazmatismProj>();

        public override int BuffType => ModContent.BuffType<TinyTwinsBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Twins");
            Tooltip.SetDefault("Summons a tiny pair of The Twins to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
