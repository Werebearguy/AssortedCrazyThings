using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class MiniAntlionItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<MiniAntlionProj>();

        public override int BuffType => ModContent.BuffType<MiniAntlionBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Antlion Egg");
            Tooltip.SetDefault("Summons a friendly Baby Antlion to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 10);
        }
    }
}
