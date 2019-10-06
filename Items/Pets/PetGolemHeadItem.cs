using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = ModContent.ProjectileType<PetGolemHeadProj>();
            item.buffType = ModContent.BuffType<PetGolemHeadBuff>();
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
