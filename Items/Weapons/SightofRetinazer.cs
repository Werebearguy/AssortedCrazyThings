using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Weapons)]
	public class SightofRetinazer : AssItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sight of Retinazer");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LaserRifle);
			Item.width = 62;
			Item.height = 32;
			Item.damage = 40;
			Item.mana = 0;
			Item.shoot = ProjectileID.MiniRetinaLaser;
			Item.shootSpeed = 15f;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = -11;
			Item.autoReuse = true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 5).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.LaserRifle, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
