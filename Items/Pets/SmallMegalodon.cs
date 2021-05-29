using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class SmallMegalodon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Megalodon Tooth");
            Tooltip.SetDefault("Summons a friendly Small Megalodon that follows you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("SmallMegalodon").Type;
            Item.buffType = Mod.Find<ModBuff>("SmallMegalodon").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(silver: 10);
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
