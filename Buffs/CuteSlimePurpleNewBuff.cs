using AssortedCrazyThings.Projectiles;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CuteSlimePurpleNewBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cute Purple Slime");
            Description.SetDefault("A cute purple slime girl is following you.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
            player.GetModPlayer<PetPlayer>(mod).CuteSlimePurpleNew = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<CuteSlimePurpleNewPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                int i = Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y, 0f, 0f, mod.ProjectileType<CuteSlimePurpleNewPet>(), 0, 0f, player.whoAmI, 0f, 0f);
                Main.projectile[i].GetGlobalProjectile<PetAccessoryProj>(mod).SetAccessoryAll(mPlayer.slotsPlayer);
                mPlayer.slimePetIndex = i;
            }
        }
    }
}
