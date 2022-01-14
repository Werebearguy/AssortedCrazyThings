using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class TorchGodLightPetBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<TorchGodLightPetProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().TorchGodLightPet;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Godly Torch");
            Description.SetDefault("Your torch placement has been deemed unnecessary. I will do it correctly.");
            Main.vanityPet[Type] = false;
            Main.lightPet[Type] = true;
        }
    }
}
