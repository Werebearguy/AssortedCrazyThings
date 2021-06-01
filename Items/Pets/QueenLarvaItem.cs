using Terraria;
using Terraria.ModLoader;
using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.Buffs.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    public class QueenLarvaItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<QueenLarvaProj>();

        public override int BuffType => ModContent.BuffType<QueenLarvaBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Queen Larva");
            Tooltip.SetDefault("Summons a Queen Bee Larva to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
