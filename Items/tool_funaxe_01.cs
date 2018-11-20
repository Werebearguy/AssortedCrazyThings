using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items
{
	public class tool_funaxe_01 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Craft of Miners");
			Tooltip.SetDefault("BREAK THOSE BLOCKS WITH YOUR BARE HANDS!");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ShroomiteDiggingClaw);
            item.useAnimation = 3;
            item.useTime = 3;
			item.value = 0;
			item.rare = -11;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ShroomiteDiggingClaw"), 5);
            recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(10))
			{
				Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
				Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
				Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
				Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
			}
		}
	}
}