using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetGolemHeadBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Replica Golem Head");
            Description.SetDefault("A Replica Golem Head watches over you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().PetGolemHead = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PetGolemHeadProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + (player.height / 2), -player.direction, -1f, ModContent.ProjectileType<PetGolemHeadProj>(), 0, 0f, player.whoAmI, 0f, (player.whoAmI * 13) % PetGolemHeadProj.AttackDelay);
            }
        }
    }
}

