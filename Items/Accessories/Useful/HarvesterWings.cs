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
            Tooltip.SetDefault("Allows slowfall" + 
			"\nIncreases Minion damage by 15%" +
			"\nIncreases Minion count by 1");
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
			player.minionDamage += 0.15f;
            player.maxMinions++;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) 
			{
				ascentWhenFalling = 0.55f;
				ascentWhenRising = 0.8f;
				maxCanAscendMultiplier = 1f;
				maxAscentMultiplier = 3f;
				constantAscend = 0.135f;
			}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration) 
		{
			speed = 9f;
			acceleration *= 2.5f;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 25);
            recipe.AddIngredient(mod.ItemType<CaughtDungeonSoulFreed>(), 25);
            recipe.AddIngredient(mod.ItemType<DesiccatedLeather>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}