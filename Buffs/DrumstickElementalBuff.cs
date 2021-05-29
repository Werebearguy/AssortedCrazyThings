using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class DrumstickElementalBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Drumstick Elemental");
            Description.SetDefault("Dinner is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().DrumstickElemental = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<DrumstickElementalProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, ModContent.ProjectileType<DrumstickElementalProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
