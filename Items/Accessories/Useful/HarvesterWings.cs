using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	[AutoloadEquip(EquipType.Wings)]
	public class HarvesterWings : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harvester Wings");
            Tooltip.SetDefault("Allows flight and slow fall" +
            "\nIncreases your max number of minions" +
            "\n10% increased summon damage");
        }

		public override void SetDefaults() 
		{
            item.width = 40;
            item.height = 28;
            item.value = Item.sellPrice(gold: 1);
            item.rare = -11;
            item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) 
		{
            player.wingTimeMax = 95;
			player.minionDamage += 0.1f;
            player.maxMinions++;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) 
		{
			ascentWhenFalling = 0.3f;
			ascentWhenRising = 0.2f;
			maxCanAscendMultiplier = 0.5f;
			maxAscentMultiplier = 2.5f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration) 
		{
			speed = 7f;
			acceleration *= 1.5f;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 25);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 50);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
