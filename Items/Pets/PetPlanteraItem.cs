using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = ModContent.ProjectileType<PetPlanteraProj>();
            item.buffType = ModContent.BuffType<PetPlanteraBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
