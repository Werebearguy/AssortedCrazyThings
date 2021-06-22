using Terraria;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [Autoload]
    [LegacyName("CuteGastropod")]
    public class CuteGastropodItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<CuteGastropodProj>();

        public override int BuffType => ModContent.BuffType<CuteGastropodBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mysterious Pod");
            Tooltip.SetDefault("Summons a friendly Cute Gastropod to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
        }
    }
}
