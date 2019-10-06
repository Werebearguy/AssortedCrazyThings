using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.CuteSlimes
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
            projType = ModContent.ProjectileType<CuteSlimeToxicNewProj>();
        }
    }
}
