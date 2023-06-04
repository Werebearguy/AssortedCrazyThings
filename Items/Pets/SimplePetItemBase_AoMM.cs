using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AssortedCrazyThings.Base.ModSupport.AoMM;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace AssortedCrazyThings.Items.Pets
{
	/// <summary>
	/// Only loaded if AoMM is enabled, handles two-way recipe, texture, pet type, name/tooltip
	/// </summary>
	[Content(ContentType.AommSupport | ContentType.OtherPets)] //Give it to the base class, as it covers most pets
	public abstract class SimplePetItemBase_AoMM<T> : SimplePetItemBase where T : SimplePetItemBase
	{
		public virtual int BaseItemType => ModContent.ItemType<T>();
		public ModItem BaseModItem => ItemLoader.GetItem(BaseItemType);

		public override int PetType => (BaseModItem as T).PetType;

		//Use base item texture
		public override string Texture => BaseModItem.Texture;

		public override LocalizedText DisplayName => AmuletOfManyMinionsApi.AppendAoMMVersion(BaseModItem.DisplayName);

		public override LocalizedText Tooltip => BaseModItem.Tooltip;

		public sealed override void SafeSetStaticDefaults()
		{
			EvenSaferSetStaticDefaults();
		}

		public virtual void EvenSaferSetStaticDefaults()
		{

		}

		public sealed override void SafeSetDefaults()
		{
			Item.CloneDefaults(BaseItemType);

			//Override new parameters manually
			Item.shoot = PetType;
			Item.buffType = BuffType;

			EvenSaferSetDefaults();
		}

		public virtual void EvenSaferSetDefaults()
		{

		}

		public sealed override void AddRecipes()
		{
			static void Create2WayRecipe(int item1, int item2)
			{
				Recipe.Create(item1)
					.AddIngredient(item2)
					.AddTile(TileID.DemonAltar)
					.Register();
			}

			var items = new int[] { Type, BaseItemType };
			for (int i = 0; i < items.Length; i++)
			{
				int item1 = items[i];
				int item2 = items[(i + 1) % items.Length];

				Create2WayRecipe(item1, item2);
			}

			SafeAddRecipes();
		}

		public virtual void SafeAddRecipes()
		{

		}

		public sealed override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 3600, true);
			return EvenSaferShoot(player, source, position, velocity, type, damage, knockback);
		}

		public virtual bool EvenSaferShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}
}
