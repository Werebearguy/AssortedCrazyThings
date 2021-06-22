using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Content(ContentType.Bosses)]
    public class CompanionDungeonSoulPetBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<CompanionDungeonSoulPetProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SoulLightPet;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Description.SetDefault("A friendly Dungeon Soul is following you"
                + "\nLight pet slot");
            Main.vanityPet[Type] = false;
            Main.lightPet[Type] = true;
        }
    }
}
