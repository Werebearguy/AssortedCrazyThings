using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeToxicNewBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Toxic Slime");
            Description.SetDefault("A cute toxic slime girl is following you");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.CuteSlimeToxicNew = true;
            projType = mod.ProjectileType<CuteSlimeToxicNewProj>();
        }
    }
}
