using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class IlluminantSlimeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<IlluminantSlimeProj>();

        public override int BuffType => ModContent.BuffType<IlluminantSlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Illuminant Slime");
            Tooltip.SetDefault("Summons a friendly Illuminant Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
