using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [LegacyName("BabyOcram")]
    public class BabyOcramItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<BabyOcramProj>();

        public override int BuffType => ModContent.BuffType<BabyOcramBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Ocram");
            Tooltip.SetDefault("Summons a miniature Ocram that follows you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 60);
        }
    }
}
