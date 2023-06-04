using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class TrueLegendaryWoodenSword : WeaponItemBase
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.CobaltSword);
			Item.width = 58;
			Item.height = 58;
			Item.rare = 7;
			Item.value = Item.sellPrice(0, 2, 0, 10); //2 gold for broken, 10 copper for legendary
			Item.shoot = ModContent.ProjectileType<TrueLegendaryWoodenSwordProj>();
			Item.shootSpeed = 10f; //fairly short range, similar to throwing knife
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			//162 for "sparks"
			//169 for just light
			int dustType = 169;
			Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType, player.velocity.X * 0.2f + (player.direction * 3), player.velocity.Y * 0.2f, 100, Color.White, 1.25f);
			dust.noGravity = true;
			dust.velocity *= 2f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<LegendaryWoodenSword>(), 1).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, player.Center + Vector2.Normalize(velocity) * 30f, velocity, type, (int)(damage * 0.8f), knockback, Main.myPlayer);
			return false;
		}
	}
}
