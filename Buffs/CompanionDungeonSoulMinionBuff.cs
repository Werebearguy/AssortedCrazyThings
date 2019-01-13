using AssortedCrazyThings.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class CompanionDungeonSoulMinionBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Freed Dungeon Soul");
            Description.SetDefault("A freed Dungeon Soul is fighting for you.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (player.ownedProjectileCounts[mod.ProjectileType<CompanionDungeonSoulMinion>()] > 0)
            {
                modPlayer.soulMinion = true;
            }
            if (!modPlayer.soulMinion)
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
