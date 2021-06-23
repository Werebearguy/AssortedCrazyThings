using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.HostileNPCs | ContentType.DroppedPets)]
    [LegacyName("MiniMegalodon")]
    public class MiniMegalodonItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<MiniMegalodonProj>();

        public override int BuffType => ModContent.BuffType<MiniMegalodonBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Small Megalodon Tooth");
            Tooltip.SetDefault("Summons a friendly Mini Megalodon that follows you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 10);
        }
    }
}
