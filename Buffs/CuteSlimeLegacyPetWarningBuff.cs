using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeLegacyPetWarningBuff : CuteSlimeBaseBuff
    {
        protected override void MoreSetDefaults()
        {
            DisplayName.SetDefault("Cute Slime (Legacy)");
            Description.SetDefault("Craft the item into the proper version");
        }

        protected override void MoreUpdate(PetPlayer mPlayer)
        {
            mPlayer.LegacyPet = true;
            projType = mod.ProjectileType<CuteSlimeLegacyPetWarningProj>();
        }
    }
}
