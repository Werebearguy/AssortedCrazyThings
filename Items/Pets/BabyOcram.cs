using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class BabyOcram : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Ocram");
            Tooltip.SetDefault("Summons a miniature Ocram that follows you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("BabyOcram");
            item.buffType = mod.BuffType("BabyOcram");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 60);
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
