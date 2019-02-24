using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class SunMoonPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Personal Sun and Moon");
            Description.SetDefault("A small sun and moon are providing you with constant light"
                + "\n'No adverse gravitational effects will happen'");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).SunPet = true;
            player.GetModPlayer<PetPlayer>(mod).MoonPet = true;
            bool petProjectilesNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<SunPetProj>()] <= 0 && player.ownedProjectileCounts[mod.ProjectileType<MoonPetProj>()] <= 0;
            if (petProjectilesNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 3), 0f, 0f, mod.ProjectileType<SunPetProj>(), 0, 0f, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 3), 0f, 0f, mod.ProjectileType<MoonPetProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip += "\n" + AssUtils.GetMoonPhaseAsString();
            tip += "\n" + AssUtils.GetTimeAsString();
        }
    }
}
