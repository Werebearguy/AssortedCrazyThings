using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class ObservingEyeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<ObservingEyeProj>();

        public override int BuffType => ModContent.BuffType<ObservingEyeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Observing Eye");
            Tooltip.SetDefault("Summons a tiny version of Eye of Cthulhu to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
