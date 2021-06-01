using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class OceanSlimeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<OceanSlimeProj>();

        public override int BuffType => ModContent.BuffType<OceanSlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Ocean Slime");
            Tooltip.SetDefault("Summons a friendly Ocean Slime to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
