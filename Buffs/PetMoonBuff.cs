using AssortedCrazyThings.Projectiles.Pets;
using AssortedCrazyThings.Base;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetMoonBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Personal Moon");
            Description.SetDefault("A small moon is providing you with constant moonlight");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).PetMoon = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<PetMoonProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, mod.ProjectileType<PetMoonProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip += "\n" + AssUtils.GetMoonPhaseAsString();
        }
    }
}
