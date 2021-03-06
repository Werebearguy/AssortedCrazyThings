using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetSunMoonBuff : ModBuff
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
            player.GetModPlayer<PetPlayer>().PetSun = true;
            player.GetModPlayer<PetPlayer>().PetMoon = true;
            bool moreThanOneSun = player.ownedProjectileCounts[ModContent.ProjectileType<PetSunProj>()] > 0;
            bool moreThanOneMoon = player.ownedProjectileCounts[ModContent.ProjectileType<PetMoonProj>()] > 0;
            if (player.whoAmI == Main.myPlayer)
            {
                if (!moreThanOneSun) Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 3), 0f, 0f, ModContent.ProjectileType<PetSunProj>(), 0, 0f, player.whoAmI, 0f, 0f);
                if (!moreThanOneMoon) Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 3), 0f, 0f, ModContent.ProjectileType<PetMoonProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip += "\n" + AssUtils.GetMoonPhaseAsString();
            tip += "\n" + AssUtils.GetTimeAsString();
        }
    }
}
