using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class CompanionDungeonSoulPetItem2 : CompanionDungeonSoulPetItem
	{
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/Items/Pets/CompanionDungeonSoulPetItem"; //use fixed texture
			}
		}

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj2>();
			Item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff2>();
		}

		//hardmode recipe
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CompanionDungeonSoulPetItem>(), 1).AddTile(TileID.CrystalBall).Register();
		}
	}
	
	//Reimplementation from SimplePetItemBase_AoMM for the most part
	[Content(ContentType.AommSupport | ContentType.Bosses)]
	public class CompanionDungeonSoulPetItem2_AoMM : CompanionDungeonSoulPetItem2
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();

			string name = ModContent.GetInstance<CompanionDungeonSoulPetItem2>().Name;
			DisplayName.SetDefault("{$Mods.AssortedCrazyThings.ItemName." + name + "} {$Mods.AssortedCrazyThings.Common.AoMMVersion}");
			Tooltip.SetDefault("{$Mods.AssortedCrazyThings.ItemTooltip." + name + "}");
		}

		public override void SafeSetDefaults()
		{
			base.SafeSetDefaults();

			Item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPetProj>();
			Item.buffType = ModContent.BuffType<CompanionDungeonSoulPetBuff2_AoMM>();
		}

		public override void AddRecipes()
		{
			static void Create2WayRecipe(int item1, int item2)
			{
				Recipe.Create(item1)
					.AddIngredient(item2)
					.AddTile(TileID.DemonAltar)
					.Register();
			}

			var items = new int[] { Type, ModContent.ItemType<CompanionDungeonSoulPetItem2>() };
			for (int i = 0; i < items.Length; i++)
			{
				int item1 = items[i];
				int item2 = items[(i + 1) % items.Length];

				Create2WayRecipe(item1, item2);
			}
		}
	}
}
