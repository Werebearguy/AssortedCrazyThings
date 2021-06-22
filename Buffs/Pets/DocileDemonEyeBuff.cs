using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class DocileDemonEyeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<DocileDemonEyeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().DocileDemonEye;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Description.SetDefault("A Demon Eye is following you"
                + "\nChange its appearance with a Demon Eye Contact Case");
        }
    }
}
