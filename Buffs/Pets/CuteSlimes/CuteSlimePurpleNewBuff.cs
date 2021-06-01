using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimePurpleNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimePurpleNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimePurpleNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Purple Slime");
            Description.SetDefault("A cute purple slime girl is following you");
        }
    }
}
