using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class PlagueOfToads : WeaponItemBase
	{
		public override void SetDefaults()
		{
			Item.mana = 20;
			Item.damage = 8;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 16f;
			Item.shoot = ModContent.ProjectileType<PlagueOfToadsFired>();
			Item.width = 26;
			Item.height = 28;
			Item.UseSound = SoundID.Item66;
			Item.useAnimation = 22;
			Item.useTime = 22;
			Item.rare = 2;
			Item.noMelee = true;
			Item.knockBack = 0f;
			Item.value = Item.sellPrice(silver: 25);
			Item.DamageType = DamageClass.Magic;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, Main.MouseWorld.X, Main.MouseWorld.Y); 
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Frog, 3).AddIngredient(ItemID.WandofSparking, 1).AddTile(TileID.Anvils).Register();
		}
	}
}
