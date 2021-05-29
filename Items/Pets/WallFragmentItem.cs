using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class WallFragmentItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Fragment");
            Tooltip.SetDefault("Summons several fragments of the Wall to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.width = 22;
            Item.height = 26;
            Item.shoot = Mod.Find<ModProjectile>("WallFragmentMouth").Type;
            Item.buffType = Mod.Find<ModBuff>("WallFragmentBuff").Type;
            Item.rare = -11;
            Item.value = Item.sellPrice(copper: 10);
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
