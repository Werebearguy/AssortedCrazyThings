using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeXmasNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Christmas Slime");
            Description.SetDefault("A cute christmas slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeXmasNew = true;
            projType = ModContent.ProjectileType<CuteSlimeXmasNewProj>();
        }
    }
}
