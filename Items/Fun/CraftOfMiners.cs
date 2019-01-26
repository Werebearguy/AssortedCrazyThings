using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
	public class CraftOfMiners : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Craft of Miners");
			Tooltip.SetDefault("Use those fists of yours to tear through any blocks in your way");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ShroomiteDiggingClaw);
            item.useAnimation = 3;
            item.useTime = 3;
            item.value = Item.sellPrice(gold: 5);
            item.rare = -11;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShroomiteDiggingClaw, 5);
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