using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    [Content(ContentType.FriendlyNPCs)]
    public class JoyousSlimeItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<JoyousSlimeProj>();

        public override int BuffType => ModContent.BuffType<JoyousSlimeBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Joyous Slime");
            Tooltip.SetDefault("Summons a friendly Joyous Slime to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
