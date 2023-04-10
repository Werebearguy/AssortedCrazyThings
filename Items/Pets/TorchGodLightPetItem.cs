using AssortedCrazyThings.Base.Data;
using AssortedCrazyThings.Buffs.Pets;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
	[Content(ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilter: true)]
	public class TorchGodLightPetItem : SimplePetItemBase
	{
		public override int PetType => ModContent.ProjectileType<TorchGodLightPetProj>();

		public override int BuffType => ModContent.BuffType<TorchGodLightPetBuff>();

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(gold: 2);
		}

		public override void AddRecipes()
		{
			//Alternative if torch god favor was already used
			CreateRecipe()
				.AddIngredient(ItemID.Torch, 999)
				.AddIngredient(ItemID.LifeCrystal)
				.AddTile(TileID.DemonAltar)
				.AddCondition(AssConditions.UnlockedBiomeTorches)
				.Register();

			//Fallback
			CreateRecipe()
				.AddIngredient(ItemID.Torch, 999)
				.AddIngredient(ItemID.TorchGodsFavor)
				.AddTile(TileID.DemonAltar)
				.Register();
		}

		//TODO maybe look into reworking this into OnSpawn with GlobalItem
		public override void Load()
		{
			On_Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool += Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool;
		}

		private static int Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool(On_Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
		{
			/*
				* Try dropping when these conditions are true
				* int number = Item.NewItem(new EntitySource_TorchGod(this, "TorchGod_FavorLoot"), (int)position.X, (int)position.Y, width, height, 5043);
					if (Main.netMode == 1)
						NetMessage.SendData(21, -1, -1, null, number, 1f);
				*/
			//If this causes a recursion somehow, im screaming
			Player player = Main.LocalPlayer;
			if (source is EntitySource_TorchGod torchGodSource &&
				torchGodSource.Context == "TorchGod_FavorLoot" &&
				torchGodSource.TargetedEntity == player &&
				Type == ItemID.TorchGodsFavor && Stack == 1)
			{
				int itemToDrop = ModContent.ItemType<TorchGodLightPetItem>();
				int number = Item.NewItem(source, player.getRect(), itemToDrop);
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f);
				}
			}

			int ret = orig(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
			return ret;
		}
	}

	//Light pet, no Aomm form

	[Content(ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilter: true)]
	public class TorchGodLightPetGlobalNPC : AssGlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.TorchGod)
			{
				LeadingConditionRule neverDropsRule = new LeadingConditionRule(new Conditions.NeverTrue());
				neverDropsRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TorchGodLightPetItem>()));
				npcLoot.Add(neverDropsRule);
			}
		}
	}
}
