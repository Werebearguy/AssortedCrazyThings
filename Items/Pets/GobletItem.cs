using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class GobletItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblet Battle Standard");
            Tooltip.SetDefault("Summons a tiny goblin to follow you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("GobletProj").Type;
            Item.buffType = Mod.Find<ModBuff>("GobletBuff").Type;
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
