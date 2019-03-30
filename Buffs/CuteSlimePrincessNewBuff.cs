using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimePrincessNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Princess Slime");
            Description.SetDefault("A cute princess slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimePrincessNew = true;
            projType = mod.ProjectileType<CuteSlimePrincessNewProj>();
        }
    }
}
