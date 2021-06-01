using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeXmasNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeXmasNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeXmasNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Christmas Slime");
            Description.SetDefault("A cute christmas slime girl is following you");
        }
    }
}
