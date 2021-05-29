using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class OrigamiCrane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Origami Crane");
            Tooltip.SetDefault("Summons an Origami Crane that follows you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("OrigamiCrane").Type;
            Item.buffType = Mod.Find<ModBuff>("OrigamiCrane").Type;
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
