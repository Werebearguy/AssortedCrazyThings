using AssortedCrazyThings.Base;
using AssortedCrazyThings.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

/*
 Silly Balloon Kit
- Accessory
- Functionality similar to costume suitcase, but for vanity balloon accessories.
- Crafting recipe TBD, maybe sold by Party Girl? */
namespace AssortedCrazyThings.Items.Accessories.Vanity
{
	public enum BalloonType : byte
	{
		DemonEyeGreen,
		DemonEyePurple,
		DemonEyeRed,
		FracturedEyeGreen,
		FracturedEyePurple,
		FracturedEyeRed,
		MetalEyeGreen,
		MetalEyePurple,
		MetalEyeRed,
		FracturedMetalEyeGreen,
		FracturedMetalEyePurple,
		FracturedMetalEyeRed,
		Spazmatism,
		Retinazer,
		Cobballoon,
	}

	[AutoloadEquip(EquipType.Balloon)] //Dummy texture
	public class SillyBalloonKit : VanityAccessoryBase
	{
		public static LocalizedText Enum2string(BalloonType e)
		{
			var name = Enum.GetName(typeof(BalloonType), e);
			return ModContent.GetInstance<SillyBalloonKit>().GetLocalization(name, () => Regex.Replace(name, "([A-Z])", " $1").Trim());
		}

		//Half-assed implementation
		public static CircleUIConf GetUIConf()
		{
			List<Asset<Texture2D>> assets = new();
			List<string> tooltips = new();

			var array = Enum.GetValues(typeof(BalloonType));
			foreach (var balloon in array)
			{
				assets.Add(AssUtils.Instance.Assets.Request<Texture2D>($"Items/Accessories/Vanity/SillyBalloonKitBalloons/{Enum.GetName(typeof(BalloonType), balloon)}Preview"));
				tooltips.Add(Enum2string((BalloonType)balloon).ToString());
			}

			return new CircleUIConf(0, -1, assets, tooltips: tooltips);
		}

		public static Dictionary<BalloonType, int> EquipSlots { get; private set; }

		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipSlots = new Dictionary<BalloonType, int>();

				var array = Enum.GetValues(typeof(BalloonType));
				foreach (var balloon in array)
				{
					var name = Enum.GetName(typeof(BalloonType), balloon);
					EquipSlots[(BalloonType)balloon] = EquipLoader.AddEquipTexture(Mod, $"AssortedCrazyThings/Items/Accessories/Vanity/SillyBalloonKitBalloons/{name}_Balloon", EquipType.Balloon, name: name); 
				}
			}
		}

		public override void SafeSetStaticDefaults()
		{
			//Needs to be called so the lang is initialized
			if (!Main.dedServ)
			{
				GetUIConf();
			}
		}

		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = 1;
		}
	}
}
