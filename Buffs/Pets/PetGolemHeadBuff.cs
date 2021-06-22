using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class PetGolemHeadBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<PetGolemHeadProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().PetGolemHead;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Replica Golem Head");
            Description.SetDefault("A Replica Golem Head watches over you");
        }
    }
}

