using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Bosses)]
	public class Talonstorm : WeaponItemBase
	{
		public override void SetDefaults()
		{
			Item.mana = 30;
			Item.damage = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 14f;
			Item.shoot = ModContent.ProjectileType<TalonstormFiredProj>();
			Item.width = 46;
			Item.height = 46;
			Item.UseSound = SoundID.Item66;
			Item.useAnimation = 22;
			Item.useTime = 22;
			Item.rare = 2;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(0, 0, 57, 50); //5 silver for souls, 50 for leather, 2.5 silver for bone
			Item.DamageType = DamageClass.Magic;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 25).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 10).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddTile(TileID.Anvils).Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, Main.MouseWorld.X, Main.MouseWorld.Y);
			return false;
		}
	}
}
