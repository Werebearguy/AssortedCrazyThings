using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace Harblesnargits_Mod_01.Items.Fun
{
	public class InfinityRocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infinity Rocket");
			Tooltip.SetDefault("It seriously never ends!");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.EndlessMusketPouch);
			item.ammo = AmmoID.Rocket;
            item.rare = -11;
            item.shoot = 0;
            item.damage = 40;
            item.UseSound = SoundID.Item11;
            //item.shoot = ProjectileID.RocketII;

        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RocketI, 3996);
			recipe.AddTile(TileID.CrystalBall);  //WorkBenches, Anvils, MythrilAnvil, Furnaces, DemonAltar, or TinkerersWorkbench
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}