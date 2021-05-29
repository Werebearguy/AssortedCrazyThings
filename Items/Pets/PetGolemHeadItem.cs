using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetGolemHeadItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Replica Golem Head");
            Tooltip.SetDefault("Summons a Replica Golem Head to watch over you"
                + "\nShoots bouncing fireballs at nearby enemies");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetGolemHeadProj>();
            Item.buffType = ModContent.BuffType<PetGolemHeadBuff>();
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
