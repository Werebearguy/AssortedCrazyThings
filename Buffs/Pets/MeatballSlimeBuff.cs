using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    public class MeatballSlimeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<MeatballSlimeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().MeatballSlime;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Meatball");
            Description.SetDefault("Meatball is following you");
        }
    }
}
