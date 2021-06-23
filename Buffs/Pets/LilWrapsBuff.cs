using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.DroppedPets)]
    public class LilWrapsBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<LilWrapsProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().LilWraps;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Lil' Wraps");
            Description.SetDefault("A Mummy is following you");
        }
    }
}
