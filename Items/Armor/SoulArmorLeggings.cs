using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SoulArmorLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior's Garb");
            Tooltip.SetDefault("Soul Savior Garment"
                + "\nwhatever the fuck");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(gold:1);
            item.rare = -11;
            item.defense = 6; //TODO ADJUST HERE
        }

        public override void UpdateEquip(Player player)
        {
            //TODO
            //
        }

        public override void AddRecipes()
        {
            //TODO ADJUST HERE
        }
    }
}