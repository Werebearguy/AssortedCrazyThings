using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class HornedSlimeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<HornedSlimeProj>();

        public override int BuffType => ModContent.BuffType<HornedSlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Horned Slime");
            Tooltip.SetDefault("Summons a friendly Horned Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
