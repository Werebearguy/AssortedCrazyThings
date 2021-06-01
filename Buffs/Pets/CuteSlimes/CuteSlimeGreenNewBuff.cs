using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeGreenNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeGreenNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeGreenNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Green Slime");
            Description.SetDefault("A cute green slime girl is following you");
        }
    }
}
