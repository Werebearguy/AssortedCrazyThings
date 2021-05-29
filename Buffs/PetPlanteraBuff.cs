using AssortedCrazyThings.Base;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class PetPlanteraBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Plantera Sprout");
            Description.SetDefault("A Plantera Sprout is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().PetPlantera = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PetPlanteraProj>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, ModContent.ProjectileType<PetPlanteraProj>(), PetPlanteraProj.ContactDamage, 1f, player.whoAmI, 0f, 0f);

                if (AssUtils.CountProjs(ModContent.ProjectileType<PetPlanteraProjTentacle>(), player.whoAmI) == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (player.width / 2), player.position.Y + player.height / 3, 0f, 0f, ModContent.ProjectileType<PetPlanteraProjTentacle>(), 1, 0f, player.whoAmI, 0f, 0f);
                    }
                }
            }
        }
    }
}
