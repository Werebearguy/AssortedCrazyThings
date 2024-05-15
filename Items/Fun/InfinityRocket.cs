using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Fun
{
	[Content(ContentType.Weapons)]
	public class InfinityRocket : AssItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.EndlessMusketPouch);
			Item.ammo = AmmoID.Rocket;
			Item.rare = 8;
			Item.shoot = ProjectileID.None;
			Item.damage = 40;
			Item.UseSound = SoundID.Item11;
			Item.value = Item.sellPrice(gold: 4);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.RocketI, 3996).AddTile(TileID.CrystalBall).Register();
		}
	}

	//Fix for Celebration Mk2 and other fancy launchers
	[Content(ContentType.Weapons)]
	public class InfinityRocketSystem : AssSystem
	{
		public override void PostSetupContent()
		{
			var itemType = ModContent.ItemType<InfinityRocket>();
			foreach (var pair in AmmoID.Sets.SpecificLauncherAmmoProjectileMatches)
			{
				var dict = pair.Value;
				if (dict.TryGetValue(ItemID.RocketI, out var proj))
				{
					dict[itemType] = proj;
				}
				else
				{
					dict[itemType] = ProjectileID.RocketI;
				}
			}
		}
	}
}
