using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.Bosses)]
	public class CompanionDungeonSoulPetItem : CaughtDungeonSoulBase
	{
		public static LocalizedText CommonTooltipText { get; private set; }

		public override void SetStaticDefaults()
		{
			CommonTooltipText = Language.GetOrRegister(Mod.GetLocalizationKey($"Items.{nameof(CompanionDungeonSoulPetItem)}.CommonTooltip"));

			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			ItemID.Sets.ItemNoGravity[Item.type] = true;

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SafeSetDefaults()
		{
			Item.DefaultToVanitypet(ModContent.ProjectileType<CompanionDungeonSoulPetProj>(), ModContent.BuffType<CompanionDungeonSoulPetBuff>());
			frame2CounterCount = -1;
			animatedTextureSelect = 0;

			Item.width = 26;
			Item.height = 28;
			Item.maxStack = 1;
			Item.noUseGraphic = true;

			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 50);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 3600);
			return false;
		}

		//hardmode recipe
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 1).AddIngredient(ModContent.ItemType<DesiccatedLeather>(), 1).AddIngredient(ItemID.Bone, 2).AddTile(TileID.CrystalBall).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem2>(), 1).AddTile(TileID.CrystalBall).Register();
		}
	}

	//Light pet, no Aomm form
}
