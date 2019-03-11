using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
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
            //this is for the altTexture thing where you can specify which slime has which texture
            color = (byte)CuteSlimeBaseProj.PetColor.Crimson;
        }
    }
}
