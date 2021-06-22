using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.FriendlyNPCs)]
    [LegacyName("YoungHarpy")]
    public class YoungHarpyItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<YoungHarpyProj>();

        public override int BuffType => ModContent.BuffType<YoungHarpyBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clump of Down Feathers");
            Tooltip.SetDefault("Summons a friendly Harpy to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
