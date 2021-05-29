using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class OrigamiCrane : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Origami Crane");
            Description.SetDefault("An Origami Crane is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<PetPlayer>().OrigamiCrane = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[Mod.Find<ModProjectile>("OrigamiCrane").Type] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, Mod.Find<ModProjectile>("OrigamiCrane").Type, 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
