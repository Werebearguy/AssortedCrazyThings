using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles
{
	[Content(ContentType.PlaceablesFunctional | ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilterOut: true)]
	public class VanityDresserTile : AssTile
	{
		public LocalizedText MouseoverText { get; private set; }
		public LocalizedText WorkProperlyText { get; private set; }

		public LocalizedText NoCostumesFoundPetText { get; private set; }
		public LocalizedText NoCostumesFoundLightPetText { get; private set; }

		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(200, 200, 200), name);
			DustType = 11;
			TileID.Sets.DisableSmartCursor[Type] = true;

			MouseoverText = this.GetLocalization("Mouseover");
			WorkProperlyText = this.GetLocalization("WorkProperly");
			NoCostumesFoundPetText = this.GetLocalization("NoCostumesFoundPet");
			NoCostumesFoundLightPetText = this.GetLocalization("NoCostumesFoundLightPet");
		}

		private void MouseOverCombined(bool close)
		{
			Player player = Main.LocalPlayer;
			player.mouseInterface = true;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type);
			player.GetModPlayer<AssPlayer>().mouseoveredDresser = true;
			if (close && player.itemAnimation == 0)
			{
				// "\n[c/"+ (Color.Orange * (Main.mouseTextColor / 255f)).Hex3() + ":\nCostume Dresser]" doesnt work cause chat tags are broken with escape characters
				player.cursorItemIconText = $"\n{MouseoverText}";
				if (player.HeldItem.type != ItemID.None)
				{
					player.cursorItemIconText += $"\n{WorkProperlyText}";
				}
			}
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOverCombined(false);
		}

		public override void MouseOver(int i, int j)
		{
			MouseOverCombined(true);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}
