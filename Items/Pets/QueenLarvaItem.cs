using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class QueenLarvaItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Queen Larva");
            Tooltip.SetDefault("Summons a Queen Bee Larva to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("QueenLarvaProj").Type;
            Item.buffType = Mod.Find<ModBuff>("QueenLarvaBuff").Type;
            Item.width = 28;
            Item.height = 32;
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
