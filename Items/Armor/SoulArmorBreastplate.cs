using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class SoulArmorBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Chest");
            Tooltip.SetDefault("Soul Savior Garment"
                + "\n+1 max minions");
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
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            //TODO ADJUST HERE
        }
    }
}