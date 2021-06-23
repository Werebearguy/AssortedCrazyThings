using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.DroppedPets)]
    public class TrueObservingEyeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<TrueObservingEyeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TrueObservingEye;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("True Observing Eye");
            Description.SetDefault("A tiny True Eye of Cthulhu is following you");
        }
    }
}
