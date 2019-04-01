using AssortedCrazyThings.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
    public class CuteSlimeSpawnEnableFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CuteSlimeSpawnEnable");
            Tooltip.SetDefault("Tooltip");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Silk);
            item.width = 26;
            item.height = 24;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.buffTime = 24000; //eight minutes
            item.buffType = mod.BuffType<CuteSlimeSpawnEnableBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(silver: 2);
        }
    }
}
