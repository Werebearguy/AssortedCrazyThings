using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
	[Content(ConfigurationSystem.AllFlags, needsAllToFilter: true)]
	public class AssRecipes : AssSystem
	{
		public static int RegularCuteSlimesGroup { get; private set; }
		public static int EvilWoodGroup { get; private set; }
		public static int GoldPlatinumGroup { get; private set; }
		public static int AdamantiteTitaniumGroup { get; private set; }
		public static int DemoniteCrimtaneGroup { get; private set; }

		public override void AddRecipeGroups()
		{
			string any = Language.GetTextValue("LegacyMisc.37") + " ";
			if (ContentConfig.Instance.CuteSlimes && ContentConfig.Instance.PlaceablesFunctional)
			{
				RegularCuteSlimesGroup = RecipeGroup.RegisterGroup("ACT:RegularCuteSlimes", new RecipeGroup(() => any + "Regular Bottled Slime", new int[]
				{
					ModContent.ItemType<CuteSlimeBlueItem>(),
					ModContent.ItemType<CuteSlimeBlackItem>(),
					ModContent.ItemType<CuteSlimeGreenItem>(),
					ModContent.ItemType<CuteSlimePinkItem>(),
					ModContent.ItemType<CuteSlimePurpleItem>(),
					ModContent.ItemType<CuteSlimeRainbowItem>(),
					ModContent.ItemType<CuteSlimeRedItem>(),
					ModContent.ItemType<CuteSlimeYellowItem>()
				}));
			}

			EvilWoodGroup = RecipeGroup.RegisterGroup(nameof(ItemID.Ebonwood), new RecipeGroup(() => any + Lang.GetItemNameValue(ItemID.Ebonwood), new int[]
			{
					ItemID.Ebonwood,
					ItemID.Shadewood
			}));

			GoldPlatinumGroup = RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), new RecipeGroup(() => any + Lang.GetItemNameValue(ItemID.GoldBar), new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			}));

			AdamantiteTitaniumGroup = RecipeGroup.RegisterGroup(nameof(ItemID.AdamantiteBar), new RecipeGroup(() => any + Lang.GetItemNameValue(ItemID.AdamantiteBar), new int[]
			{
				ItemID.AdamantiteBar,
				ItemID.TitaniumBar
			}));

			DemoniteCrimtaneGroup = RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), new RecipeGroup(() => any + Lang.GetItemNameValue(ItemID.DemoniteBar), new int[]
			{
				ItemID.DemoniteBar,
				ItemID.CrimtaneBar
			}));
		}
	}
}
