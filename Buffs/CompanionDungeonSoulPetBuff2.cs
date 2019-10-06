using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CompanionDungeonSoulPetBuff2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Description.SetDefault("A friendly Dungeon Soul is following you"
                + "\nPet slot");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().SoulLightPet2 = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<CompanionDungeonSoulPetProj2>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<CompanionDungeonSoulPetProj2>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
