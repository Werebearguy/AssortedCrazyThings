using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetSunBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Personal Sun");
            Description.SetDefault("A small sun is providing you with constant sunlight");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().PetSun = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PetSunProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, ModContent.ProjectileType<PetSunProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip += "\n" + AssUtils.GetTimeAsString();
        }
    }
}
