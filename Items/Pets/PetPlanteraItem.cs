using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetPlanteraItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Potted Plantera Seed");
            Tooltip.SetDefault("Summons a Plantera Sprout to watch over you"
                + "\n'It's a mean and green'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ModContent.ProjectileType<PetPlanteraProj>();
            Item.buffType = ModContent.BuffType<PetPlanteraBuff>();
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
