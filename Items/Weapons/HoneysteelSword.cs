using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class HoneysteelSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honeysteel Sword");
		}

		public override void SetDefaults()
		{
			item.damage = 55;
			item.melee = true;
			item.width = 58;
			item.height = 58;
			item.useTime = 26;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 1);
			item.rare = -11;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(mod.ItemType("HoneysteelBar"), 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Slow, 300);
		}
		
	}
}
