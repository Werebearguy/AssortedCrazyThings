using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
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
            projType = mod.ProjectileType<CuteSlimeJungleNewProj>();
        }
    }
}
