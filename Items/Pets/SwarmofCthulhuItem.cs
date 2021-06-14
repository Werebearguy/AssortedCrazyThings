using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class SwarmofCthulhuItem : SimplePetItemBase
    {
        public override int PetType => ModContent.ProjectileType<SwarmofCthulhuProj>();

        public override int BuffType => ModContent.BuffType<SwarmofCthulhuBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swarm of Cthulhu"); //TODO name, aquisition
            Tooltip.SetDefault("Summons a Swarm of Cthulhu to follow you");
        }

        public override void SafeSetDefaults()
        {
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
        }
    }
}
