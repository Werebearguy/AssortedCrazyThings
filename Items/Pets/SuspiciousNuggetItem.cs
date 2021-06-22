using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Autoload]
    public class SuspiciousNuggetItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<SuspiciousNuggetProj>();

        public override int BuffType => ModContent.BuffType<SuspiciousNuggetBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Suspicious Nugget");
            Tooltip.SetDefault("Summons a Suspicious Nugget to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.buyPrice(platinum: 50);
        }
    }
}
