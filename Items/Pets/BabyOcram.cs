using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("BabyOcram").Type;
            Item.buffType = Mod.Find<ModBuff>("BabyOcram").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 60);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}
