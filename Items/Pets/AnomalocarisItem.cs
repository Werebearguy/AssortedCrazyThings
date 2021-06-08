using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    //TODO Unobtainable
    public class AnomalocarisItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<AnomalocarisProj>();

        public override int BuffType => ModContent.BuffType<AnomalocarisBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ornery Shrimp");
            Tooltip.SetDefault("Summons an Anomalocaris to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.rare = -11;
            Item.value = Item.sellPrice(gold: 3); //Zephyr fish price
        }
    }
}
