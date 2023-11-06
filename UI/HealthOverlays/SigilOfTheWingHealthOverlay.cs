using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AssortedCrazyThings.UI.HealthOverlays
{
	//Mostly copied from ExampleMods Vanilla Overlay
	public class SigilOfTheWingHealthOverlay : ModResourceOverlay
	{
		// This field is used to cache vanilla assets used in the CompareAssets helper method further down in this file
		private readonly Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();

		const string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";
		const string barsFolder = "Images/UI/PlayerResourceSets/HorizontalBars/";
		const string path = "AssortedCrazyThings/UI/HealthOverlays/SigilOfTheWing/";

		private Asset<Texture2D> defaultAsset;
		private Asset<Texture2D> newAsset;
		private Asset<Texture2D> horizontalBarsAsset;

		public override void Load()
		{
			defaultAsset = ModContent.Request<Texture2D>(path + "Default");
			newAsset = ModContent.Request<Texture2D>(path + "New");
			horizontalBarsAsset = ModContent.Request<Texture2D>(path + "HorizontalBars");
		}

		public override void Unload()
		{
			defaultAsset = null;
			newAsset = null;
			horizontalBarsAsset = null;
		}

		public override void PostDrawResource(ResourceOverlayDrawContext context)
		{
			if (!CanHealthOverlayDraw())
			{
				return;
			}

			Asset<Texture2D> asset = context.texture;

			Asset<Texture2D> overlayAsset = null; //Other assets like panels etc also draw, so we only need to filter out the hearts
			if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2)
			{
				// Draw over the Classic hearts
				overlayAsset = defaultAsset;
			}
			else if (CompareAssets(asset, fancyFolder + "Heart_Fill") || CompareAssets(asset, fancyFolder + "Heart_Fill_B"))
			{
				// Draw over the Fancy hearts
				overlayAsset = newAsset;
			}
			else if (CompareAssets(asset, barsFolder + "HP_Fill") || CompareAssets(asset, barsFolder + "HP_Fill_Honey"))
			{
				// Draw over the Bars life bars
				overlayAsset = horizontalBarsAsset;
			}

			if (overlayAsset == null)
			{
				return;
			}

			context.texture = overlayAsset;
			context.Draw();
		}

		// This is a helper method for checking if a certain vanilla asset was drawn
		private bool CompareAssets(Asset<Texture2D> existingAsset, string compareAssetPath)
		{
			if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
			{
				asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(compareAssetPath);
			}

			return existingAsset == asset;
		}

		public static bool CanHealthOverlayDraw()
		{
			Player player = Main.LocalPlayer;

			if (player.ghost)
			{
				return false;
			}

			AssPlayer assPlayer = player.GetModPlayer<AssPlayer>();
			return assPlayer.sigilOfTheWingOngoing;
		}
	}
}
