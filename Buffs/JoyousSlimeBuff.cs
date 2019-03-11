using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class JoyousSlimeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Joyous Slime");
            Description.SetDefault("A Joyous Slime is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>(mod).JoyousSlime = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType<JoyousSlimeProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, mod.ProjectileType<JoyousSlimeProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
