using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class SkeletronPrimeHandItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Prime's Spare Hand");
            Tooltip.SetDefault("Summons Skeletron Prime's Hand attached to you");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<SkeletronPrimeHandProj>();
            Item.buffType = ModContent.BuffType<SkeletronPrimeHandBuff>();
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
