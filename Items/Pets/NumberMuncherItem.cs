using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class NumberMuncherItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<NumberMuncherProj>();

        public override int BuffType => ModContent.BuffType<NumberMuncherBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archaic Storage Device");
            Tooltip.SetDefault("Summons an educated amphibian that follows you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 10); //TODO value
        }

        //TODO obtain Craft '1' Statue + '1' Statue at a Bookcase
    }
}
