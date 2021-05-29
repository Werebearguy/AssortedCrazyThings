using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteGastropod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mysterious Pod");
            Tooltip.SetDefault("Summons a friendly Cute Gastropod to follow you");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = Mod.Find<ModProjectile>("CuteGastropod").Type;
            Item.buffType = Mod.Find<ModBuff>("CuteGastropod").Type;
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