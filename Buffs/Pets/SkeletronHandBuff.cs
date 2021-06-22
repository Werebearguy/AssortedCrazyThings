using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class SkeletronHandBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<SkeletronHandProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().SkeletronHand;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Skeletron's Spare Hand");
            Description.SetDefault("Skeletron's Hand is attached to you");
        }
    }
}
