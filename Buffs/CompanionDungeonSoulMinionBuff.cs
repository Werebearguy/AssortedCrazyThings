using AssortedCrazyThings.Projectiles.Minions;
using System;
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

        private int SumOfSoulCounts(Player player)
        {
            int sum = 0;
            foreach(CompanionDungeonSoulMinionBase.SoulType soulType in Enum.GetValues(typeof(CompanionDungeonSoulMinionBase.SoulType)))
            {
                //Main.NewText((int)soulType + " " + player.ownedProjectileCounts[CompanionDungeonSoulMinionBase.GetAssociatedStats(mod, (int)soulType).Type]);
                sum += player.ownedProjectileCounts[CompanionDungeonSoulMinionBase.GetAssociatedStats(mod, (int)soulType).Type];
            }
            return sum;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (SumOfSoulCounts(player) > 0)
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
