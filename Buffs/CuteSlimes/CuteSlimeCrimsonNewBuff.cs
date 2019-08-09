using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;

namespace AssortedCrazyThings.Buffs.CuteSlimes
{
    public class CuteSlimeCrimsonNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Crimson Slime");
            Description.SetDefault("A cute crimson slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeCrimsonNew = true;
            projType = mod.ProjectileType<CuteSlimeCrimsonNewProj>();
        }
    }
}
