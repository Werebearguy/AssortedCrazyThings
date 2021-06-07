using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class PetHarvesterBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<PetHarvesterProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetHarvester;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Stubborn Bird Soul");
            Description.SetDefault("A stubborn bird soul is following you");
        }
    }
}
