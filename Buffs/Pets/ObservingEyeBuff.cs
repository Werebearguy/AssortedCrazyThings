using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class ObservingEyeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<ObservingEyeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().ObservingEye;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Observing Eye");
            Description.SetDefault("A Tiny Eye Of Cthulhu is following you");
        }
    }
}
