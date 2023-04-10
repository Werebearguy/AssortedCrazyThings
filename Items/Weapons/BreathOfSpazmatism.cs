using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class BreathOfSpazmatism : WeaponItemBase
	{
		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Breath of Spazmatism");
			/* Tooltip.SetDefault("Uses gel to fire a stream of cursed flames"+
				"\n80% chance to not consume ammo"); */
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Flamethrower);
			Item.width = 72;
			Item.height = 38;
			//item.damage = 20; //same damage as flamethrower, which is 35
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<SpazmatismFire>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Gel;
			Item.useTime = 3; //adjusted from 10 to 3 to match spazmatism speed
			Item.useAnimation = 3; //^
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(gold: 15, silver: 20);
			Item.rare = 4;
			Item.autoReuse = true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 5).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.Flamethrower, 1).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= .80f; //80% chance not to consume ammo (since its so fast)
		}
	}
}
