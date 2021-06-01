using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets.CuteSlimes
{
    public class CuteSlimeRedNewBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CuteSlimeRedNewProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().CuteSlimeRedNew;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Cute Red Slime");
            Description.SetDefault("A cute red slime girl is following you");
        }
    }
}
