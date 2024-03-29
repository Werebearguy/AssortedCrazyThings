using AssortedCrazyThings.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	public class BreathOfSpazmatism : WeaponItemBase
	{
		public static readonly int SaveAmmoChance = 80;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(SaveAmmoChance);

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Flamethrower);
			Item.width = 72;
			Item.height = 38;
			//item.damage = 20; //same damage as flamethrower, which is 35
			Item.UseSound = SoundID.Item34;
			Item.shoot = ModContent.ProjectileType<BreathOfSpazmatismProj>();
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
			return Main.rand.NextFloat() >= SaveAmmoChance / 100f; //80% chance not to consume ammo (since its so fast)
		}
	}
}
