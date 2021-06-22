using Terraria;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.HostileNPCs)]
    public class AnimatedTomeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<AnimatedTomeProj>();

        public override int BuffType => ModContent.BuffType<AnimatedTomeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Tome");
            Tooltip.SetDefault("Summons a friendly Animated Tome to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
