using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetQueenSlimeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<PetQueenSlimeAirProj>();

        public override int BuffType => ModContent.BuffType<PetQueenSlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Playful Slimelings");
            Tooltip.SetDefault("Summons a trio of slimelings to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.buyPrice(copper: 10);
        }
    }
}
