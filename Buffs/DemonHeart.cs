using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class DemonHeart : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Demon Heart");
            Description.SetDefault("A Demon Heart is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).DemonHeart = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("DemonHeart")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("DemonHeart"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
