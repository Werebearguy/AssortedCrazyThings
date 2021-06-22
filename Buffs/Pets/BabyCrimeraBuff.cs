using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class BabyCrimeraBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<BabyCrimeraProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().BabyCrimera;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Baby Crimera");
            Description.SetDefault("A little Crimera is following you");
        }
    }
}
