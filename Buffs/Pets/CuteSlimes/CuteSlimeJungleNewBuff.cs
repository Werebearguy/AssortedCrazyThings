using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeJungleNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeJungleNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeJungleNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Jungle Slime");
            Description.SetDefault("A cute jungle slime girl is following you");
        }
    }
}
