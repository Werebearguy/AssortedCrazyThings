using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeCrimsonNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeCrimsonNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeCrimsonNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Crimson Slime");
            Description.SetDefault("A cute crimson slime girl is following you");
        }
    }
}
