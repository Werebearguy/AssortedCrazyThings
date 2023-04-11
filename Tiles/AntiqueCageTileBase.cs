using AssortedCrazyThings.Projectiles.NPCs.Bosses.Harvester;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles
{
	[Content(ContentType.Bosses)]
	public abstract class AntiqueCageTileBase : AssTile
	{
		public const int Width = 3;
		public const int Height = 3;
		public const int FrameHeight = 18 * Height;

		public static HashSet<int> InteractableCageTypes { get; private set; }

		public static LocalizedText CommonMapEntryText { get; private set; }

		public static bool IsTileInteractable(int i, int j)
		{
			return Framing.GetTileSafely(i, j) is Tile tile && InteractableCageTypes.Contains(tile.TileType);
		}

		public override void Load()
		{
			InteractableCageTypes = new HashSet<int>();
		}

		public override void Unload()
		{
			InteractableCageTypes = null;
		}

		public sealed override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			DustType = 1;

			CommonMapEntryText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{LocalizationCategory}.AntiqueCage.MapEntry"));
			AddMapEntry(new Color(102, 115, 103), CommonMapEntryText);
			
			//Whatever is specified as anchors here has to be checked in a GlobalTile/Wall class to prevent destruction

			//Workaround since chains are not valid anchors as they aren't solid
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.addTile(Type);

			//Chandelier style, with some experiment of "place anywehere" which silently exceptions when placing into tiles
			//TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			//TileObjectData.newTile.Origin = new Point16(1, 0);
			//TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			//TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			////TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			////TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
			////TileObjectData.newTile.AnchorRight = AnchorData.Empty;
			//TileObjectData.newTile.Width = Width;
			//TileObjectData.newTile.Height = Height;
			//TileObjectData.newTile.StyleHorizontal = true;
			//TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			//TileObjectData.addTile(Type);

			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 2 : 4;
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if (IsTileInteractable(i, j))
			{
				return false;
			}

			return base.CanKillTile(i, j, ref blockDamaged);
		}

		public override bool CanExplode(int i, int j)
		{
			if (IsTileInteractable(i, j))
			{
				return false;
			}

			return base.CanExplode(i, j);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (InteractableCageTypes.Contains(Type)) //IsTileInteractable doesnt work here
			{
				BabyHarvesterHandler.CanHarvesterSpawnNaturally = true;
			}
		}
	}

	//Demon altar type protection for anchors so they can't break
	[Content(ContentType.Bosses)]
	public class AntiqueCageGlobalWall : AssGlobalWall
	{
		public override void KillWall(int i, int j, int type, ref bool fail)
		{
			if (AntiqueCageTileBase.IsTileInteractable(i, j))
			{
				fail = true;
			}
		}

		public override bool CanExplode(int i, int j, int type)
		{
			if (AntiqueCageTileBase.IsTileInteractable(i, j))
			{
				return false;
			}

			return base.CanExplode(i, j, type);
		}
	}
}
