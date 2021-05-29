using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("BrainofConfusion").Type;
            Item.buffType = Mod.Find<ModBuff>("BrainofConfusion").Type;
            Item.rare = -11;
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
