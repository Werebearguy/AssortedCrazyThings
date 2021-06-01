using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeLavaNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeLavaNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeLavaNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Lava Slime");
            Description.SetDefault("A cute lava slime girl is following you");
        }
    }
}
