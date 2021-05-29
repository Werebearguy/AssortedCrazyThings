using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class AnimatedTomeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Animated Tome");
            Description.SetDefault("An animated tome is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().AnimatedTome = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[Mod.Find<ModProjectile>("AnimatedTomeProj").Type] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, Mod.Find<ModProjectile>("AnimatedTomeProj").Type, 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
