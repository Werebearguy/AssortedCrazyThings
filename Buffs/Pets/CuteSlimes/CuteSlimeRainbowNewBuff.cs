using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeRainbowNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeRainbowNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeRainbowNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Rainbow Slime");
            Description.SetDefault("A cute rainbow slime girl is following you");
        }
    }
}
