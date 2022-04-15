using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles
{
    [Content(ContentType.PlaceablesFunctional | ContentType.DroppedPets | ContentType.OtherPets, needsAllToFilter: true)]
    public class VanityDresserTile : DroppableTile<VanityDresserItem>
    {
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
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Costume Dresser");
            AddMapEntry(new Color(200, 200, 200), name);
            DustType = 11;
            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        private void MouseOverCombined(bool close)
        {
            Player player = Main.LocalPlayer;
            player.mouseInterface = true;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<VanityDresserItem>();
            player.GetModPlayer<AssPlayer>().mouseoveredDresser = true;
            if (close && player.itemAnimation == 0)
            {
                // "\n[c/"+ (Color.Orange * (Main.mouseTextColor / 255f)).Hex3() + ":\nCostume Dresser]" doesnt work cause chat tags are broken with escape characters
                player.cursorItemIconText = "\nCostume Dresser"
                     + "\nLeft Click to change your Pet's appearance"
                     + "\nRight Click to change your Light Pet's appearance";
                if (player.HeldItem.type != ItemID.None)
                {
                    player.cursorItemIconText += "\nFor this to work properly, don't have any item selected";
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
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ItemType);
        }
    }
}
