using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class MiniAntlionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Baby Antlion");
            Description.SetDefault("A Baby Antlion is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().MiniAntlion = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MiniAntlionProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, ModContent.ProjectileType<MiniAntlionProj>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
