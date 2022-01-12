using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.HostileNPCs)]
    public class MetroidPetBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<MetroidPetProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MetroidPet;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Metroid");
            Description.SetDefault("A space parasite is following you");
        }
    }
}
