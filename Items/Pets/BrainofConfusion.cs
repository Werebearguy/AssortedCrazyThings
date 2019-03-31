using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class BrainofConfusion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain of Confusion");
            Tooltip.SetDefault("Summons a Brain of Confusion to follow aimlessly behind you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("BrainofConfusion");
            item.buffType = mod.BuffType("BrainofConfusion");
            item.rare = -11;
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
