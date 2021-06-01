using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class PetCultistBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<PetCultistProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetCultist;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Dwarf Cultist");
            Description.SetDefault("A tiny Cultist is following you");
        }
    }
}
