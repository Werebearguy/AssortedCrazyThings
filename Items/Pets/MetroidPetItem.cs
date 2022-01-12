using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.HostileNPCs)]
    public class MetroidPetItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<MetroidPetProj>();

        public override int BuffType => ModContent.BuffType<MetroidPetBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parasite Container");
            Tooltip.SetDefault("Summons a space parasite to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }

        //TODO drop from betsy
    }
}
