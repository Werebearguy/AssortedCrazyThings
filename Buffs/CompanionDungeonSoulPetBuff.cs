using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CompanionDungeonSoulPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Companion Soul");
            Description.SetDefault("A friendly Dungeon Soul is following you"
                + "\nLight pet slot");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().SoulLightPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<CompanionDungeonSoulPetProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<CompanionDungeonSoulPetProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
