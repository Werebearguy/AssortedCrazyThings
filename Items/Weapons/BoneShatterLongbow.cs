using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[Content(ContentType.Bosses)]
	public class BoneShatterLongbow : WeaponItemBase
	{
		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Bone-Shatter Longbow");
			// Tooltip.SetDefault("Wooden arrows ignite into burning souls");
		}

		public override void SetDefaults()
		{
			Item.DefaultToBow(26, 7f, true);
			Item.damage = 23;
			Item.knockBack = 5f;
			Item.width = 30;
			Item.height = 48;
			Item.value = Item.sellPrice(0, 0, 57, 50); //5 silver for souls, 50 for leather, 2.5 silver for bone
			Item.rare = 2;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 25).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 10).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddTile(TileID.Anvils).Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ModContent.ProjectileType<BoneShatterLongbowProj>();
			}
		}
	}
}
