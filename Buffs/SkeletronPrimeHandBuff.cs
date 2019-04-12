using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class SkeletronPrimeHandBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Skeletron Prime's Spare Hand");
            Description.SetDefault("Skeletron Prime's Hand is attached to you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).SkeletronPrimeHand = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<SkeletronPrimeHandProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 2), -player.direction, -1f, mod.ProjectileType<SkeletronPrimeHandProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}