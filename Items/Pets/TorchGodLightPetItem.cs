using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	public class TorchGodLightPetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TorchGodLightPetProj>();

		public override int BuffType => ModContent.BuffType<TorchGodLightPetBuff>();

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Godly Torch");
			Tooltip.SetDefault("Summons a godly torch to follow you\n" +
				"Automatically places your normal torches with 'Smart Cursor' active");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(gold: 2);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Torch, 999)
				.AddIngredient(ItemID.LifeCrystal)
				.AddTile(TileID.DemonAltar)
				.AddCondition(new Recipe.Condition(NetworkText.FromLiteral("If Torch God's Favor was already used"), (Recipe recipe) => Main.LocalPlayer.unlockedBiomeTorches))
				.Register();
		}

		public override void Load()
		{
			On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool += Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool;
		}

		private int Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool(On.Terraria.Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
		{
			int ret = orig(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);

			/*
			 * Try dropping when these conditions are true
			 * int number = Item.NewItem(new EntitySource_ByItemSourceId(this, 6), (int)position.X, (int)position.Y, width, height, 5043);
					if (Main.netMode == 1)
						NetMessage.SendData(21, -1, -1, null, number, 1f);
			 */
			//If this causes a recursion somehow, im screaming
			Player player = Main.LocalPlayer;
			if (source is EntitySource_ByItemSourceId byItemSourceId &&
				byItemSourceId.Entity == player && byItemSourceId.SourceId == ItemSourceID.TorchGod &&
				Type == ItemID.TorchGodsFavor && Stack == 1)
			{
				int number = Item.NewItem(new EntitySource_ByItemSourceId(player, ItemSourceID.TorchGod), player.getRect(), ModContent.ItemType<TorchGodLightPetItem>());
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
				}
			}

			return ret;
		}
	}
}
