using AssortedCrazyThings.Projectiles;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimeBlackNewBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cute Black Slime");
            Description.SetDefault("A cute black slime girl is following you.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
            player.GetModPlayer<PetPlayer>(mod).CuteSlimeBlackNew = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<CuteSlimeBlackNewPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                int i = Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y, 0f, 0f, mod.ProjectileType<CuteSlimeBlackNewPet>(), 0, 0f, player.whoAmI, 0f, 0f);
                Main.projectile[i].GetGlobalProjectile<AssGlobalProjectile>(mod).SetAccessoryAll(mPlayer.slotsPlayer);
            }
        }
    }
}