using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class BreathOfSpazmatism : WeaponItemBase
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Flamethrower);
			/*
			useStyle = 5;
			autoReuse = true;
			useAnimation = 30;
			useTime = 6;
			width = 50;
			height = 18;
			shoot = 85;
			useAmmo = AmmoID.Gel;
			UseSound = SoundID.Item34;
			damage = 35;
			knockBack = 0.3f;
			shootSpeed = 7f;
			noMelee = true;
			value = 500000;
			rare = 5;
			ranged = true;
			 */
			//same damage as flamethrower, which is 35
			Item.damage += 3; //Give it slightly more damage to incentivise braindead players to pick it over the regular flamethrower
			Item.width = 72;
			Item.height = 38;
			Item.shoot = ModContent.ProjectileType<BreathOfSpazmatismProj>();
			Item.shootSpeed = 8f;
			Item.value = Item.sellPrice(gold: 15, silver: 20);
			Item.rare = 5;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0, 0);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y += player.gravDir * 4;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 5).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.Flamethrower, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
