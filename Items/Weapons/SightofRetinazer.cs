using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class SightofRetinazer : WeaponItemBase
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LaserRifle);
			Item.width = 62;
			Item.height = 32;
			Item.damage = 40;
			Item.mana = 0;
			Item.shoot = ModContent.ProjectileType<SightofRetinazerLaser>();
			Item.shootSpeed = 15f;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 4;
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
