using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class TrueObservingEyeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Observing Eye");
            Tooltip.SetDefault("Summons a True Eye of Cthulhu to watch after you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType<TrueObservingEyeProj>();
            item.buffType = mod.BuffType<TrueObservingEyeBuff>();
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
