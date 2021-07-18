using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    [Content(ContentType.Bosses)]
    public class CompanionDungeonSoulMinionBuff : AssBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Companion Soul Minion");
            Description.SetDefault("A friendly Soul is fighting for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        private int SumOfSoulCounts(Player player)
        {
            int sum = 0;
            foreach (SoulType type in Enum.GetValues(typeof(SoulType)))
            {
                if (type != SoulType.None)
                {
                    sum += player.ownedProjectileCounts[EverhallowedLantern.GetSoulData(type).ProjType];
                }
            }

            sum += player.ownedProjectileCounts[ModContent.ProjectileType<CompanionDungeonSoulPreWOFMinion>()];
            return sum;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>();
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

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            int ownedCount;
            foreach (SoulType type in Enum.GetValues(typeof(SoulType)))
            {
                if (type != SoulType.None)
                {
                    SoulData data = EverhallowedLantern.GetSoulData(type);
                    ownedCount = Main.LocalPlayer.ownedProjectileCounts[data.ProjType];
                    if (ownedCount > 0)
                    {
                        string name = data.Name;
                        int startIndex = name.IndexOf("Soul");
                        name = name.Insert(startIndex + 4, "s");
                        tip += "\n" + name + " : " + ownedCount;
                    }
                }
            }
            ownedCount = Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<CompanionDungeonSoulPreWOFMinion>()];
            if (ownedCount > 0)
            {
                tip += "\n" + "Tiny Dungeon Souls: " + ownedCount;
            }
        }
    }
}
