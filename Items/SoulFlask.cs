using AssortedCrazyThings.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    class SoulFlask: ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Flask");
            Tooltip.SetDefault("You get stronger the longer you don't receive damage.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.buffTime = 5400;
            item.buffType = mod.BuffType<EmpoweringBuff>();
            item.rare = -11;
            item.value = Item.buyPrice(gold: 1);
        }
    }
}
