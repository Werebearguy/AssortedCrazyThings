using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeJungleNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Jungle Slime");
            Description.SetDefault("A cute jungle slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeJungleNew = true;
            projType = ModContent.ProjectileType<CuteSlimeJungleNewProj>();
        }
    }
}
