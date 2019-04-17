using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class MiniAntlionItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Antlion Egg");
            Tooltip.SetDefault("Summons a friendly Baby Antlion to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType<MiniAntlionProj>();
            item.buffType = mod.BuffType<MiniAntlionBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
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
