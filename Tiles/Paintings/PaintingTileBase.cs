using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles.Paintings
{
	/// <summary>
	/// Base class for all painting tiles
	/// </summary>
	[Content(ContentType.PlaceablesDecorative)]
	public abstract class PaintingTileBase : AssTile
	{
		//TODO style classes based on dimension
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.addTile(Type);
			DustType = 7;

			AddMapEntry(new Color(99, 50, 30), Language.GetText("MapObject.Painting"));
		}
	}
}
