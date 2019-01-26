using AssortedCrazyThings.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class SlimePackMinionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Slime");
            Description.SetDefault("A friendly Slime from your Knapsack is fighting for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (player.ownedProjectileCounts[mod.ProjectileType<SlimePackMinion>()] > 0)
            {
                modPlayer.slimePackMinion = true;
            }
            if (!modPlayer.slimePackMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
