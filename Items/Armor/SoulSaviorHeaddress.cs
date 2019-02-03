using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulSaviorHeaddress : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Headdress");
            Tooltip.SetDefault("Increases minion damage by 10%"
                + "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 28;
            item.value = Item.sellPrice(copper: 50);
            item.rare = 2;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.minionDamage += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType<SoulSaviorPlate>() && legs.type == mod.ItemType<SoulSaviorRobe>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "You will reflect damage based on the number of total available minion slots"; //TODO something shorter
            player.GetModPlayer<AssPlayer>().soulSaviorArmor = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 999);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
