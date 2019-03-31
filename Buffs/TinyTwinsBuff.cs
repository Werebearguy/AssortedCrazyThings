using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class TinyTwinsBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tiny Twins");
            Description.SetDefault("The Twins are watching you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).TinyTwins = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<TinyRetinazerProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 2), 0f, 0f, mod.ProjectileType<TinySpazmatismProj>(), 0, 0f, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y, 0f, 0f, mod.ProjectileType<TinyRetinazerProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
