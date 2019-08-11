using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles
{
    public class VanityDresserTile : ModTile
    {
        public override void SetDefaults() {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Costume Dresser");
            AddMapEntry(new Color(200, 200, 200), name);
            dustType = 11;
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Dressers };
        }

        private void MouseOverCombined(bool close)
        {
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.showItemIcon = true;
            Main.LocalPlayer.showItemIcon2 = mod.ItemType<VanityDresserItem>();
            if (close)
            {
                if (Main.LocalPlayer.HeldItem.type == 0)
                {
                    Main.LocalPlayer.showItemIconText = "\nLeft Click to change your Pet's appearance"
                         + "\nRight Click to change your Light Pet's appearance";
                }
                else
                {
                    Main.LocalPlayer.showItemIconText = "\nCostume Dresser" +
                        "\nFor this to work properly, don't have any item selected";
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

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 32, mod.ItemType<VanityDresserItem>());
        }
    }
}
