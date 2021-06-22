using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    [Autoload]
    public class FairySlimeBuff : SimplePetBuffBase
    {
        public override int PetType => ModContent.ProjectileType<FairySlimeProj>();

        public override ref bool PetBool(Player player) => ref player.GetModPlayer<PetPlayer>().FairySlime;

        public override void SafeSetDefaults()
        {
            DisplayName.SetDefault("Fairy Slime");
            Description.SetDefault("A Fairy Slime is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
    }
}
