using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class PetEaterofWorldsItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Eater of worlds");
            Tooltip.SetDefault("Summons a tiny Eater of Worlds to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("PetEaterofWorldsHead");
            item.buffType = mod.BuffType("PetEaterofWorldsBuff");
            item.rare = -11;
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
