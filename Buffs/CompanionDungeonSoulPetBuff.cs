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
            Description.SetDefault("An friendly Dungeon Soul is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).SoulLightPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<CompanionDungeonSoulPetProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType<CompanionDungeonSoulPetProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
