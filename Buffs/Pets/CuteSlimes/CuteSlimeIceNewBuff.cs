using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeIceNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeIceNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeIceNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Ice Slime");
            Description.SetDefault("A cute ice slime girl is following you");
        }
    }
}
