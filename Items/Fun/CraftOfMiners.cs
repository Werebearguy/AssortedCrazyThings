using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Fun
{
	[Content(ContentType.Weapons | ContentType.Tools)]
	public class CraftOfMiners : AssItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ShroomiteDiggingClaw);
			Item.damage = 13;
			Item.useAnimation = 3;
			Item.useTime = 3;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = 9;
			Item.noUseGraphic = true;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.ShroomiteDiggingClaw, 5).AddTile(TileID.CrystalBall).Register();
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
