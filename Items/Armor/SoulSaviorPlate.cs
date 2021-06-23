using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
    [Content(ContentType.Bosses | ContentType.Armor)]
    [AutoloadEquip(EquipType.Body)]
    public class SoulSaviorPlate : AssItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul Savior Breastplate");
            Tooltip.SetDefault("Increases minion damage by 10%"
                + "\nIncreases your max number of minions by 2");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 3, silver: 70);
            Item.rare = -11;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Ectoplasm, 4).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 24).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
