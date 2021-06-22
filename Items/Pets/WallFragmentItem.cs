using Terraria;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.DroppedPets)]
    public class WallFragmentItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<WallFragmentMouth>();

        public override int BuffType => ModContent.BuffType<WallFragmentBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Fragment");
            Tooltip.SetDefault("Summons several fragments of the Wall to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
