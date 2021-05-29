using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class MiniMegalodon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Small Megalodon Tooth");
            Tooltip.SetDefault("Summons a friendly Mini Megalodon that follows you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("MiniMegalodon").Type;
            Item.buffType = Mod.Find<ModBuff>("MiniMegalodon").Type;
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
