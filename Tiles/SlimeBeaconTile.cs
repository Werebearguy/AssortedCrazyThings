using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace AssortedCrazyThings.Tiles
{
    public class SlimeBeaconTile : DroppableTile<SlimeBeaconItem>
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Slime Beacon");
            AddMapEntry(new Color(75, 139, 166), name);
            DustType = 1;
            AnimationFrameHeight = 56;
            //DisableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, ItemType);
            AssWorld.DisableSlimeRainSky();
        }

        //you need these four things for the outline to work:
        //_Highlight.png
        //TileID.Sets.HasOutlines[Type] = true;
        //DisableSmartCursor = true;
        //and this hook
        public override bool HasSmartInteract()
        {
            return true;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (AssWorld.slimeRainSky || Main.slimeRain)
            {
                if (++frameCounter >= 8)
                {
                    frameCounter = 0;
                    frame = (++frame - 1) % 8 + 1; //go from frame 1 to 8
                }
            }
            else
            {
                frame = 0;
            }
        }

        //Testing hooks
        //public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        //{
        //    Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        //}

        //public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        //{
        //    Vector2 pos = new Vector2(Main.offScreenRange);
        //    if (Main.drawToScreen)
        //    {
        //        pos = Vector2.Zero;
        //    }

        //    Tile tile = Main.tile[i, j];
        //    int type = tile.type;

        //    if (tile.frameX != 0 && tile.frameY != 0)
        //    {
        //        return;
        //    }

        //    pos.X += i * 16 - (int)Main.screenPosition.X;//i*16 world coords left of block
        //    int yOff = TileObjectData.GetTileData(type, 0)?.DrawYOffset ?? 0;
        //    pos.Y += j * 16 + yOff - (int)Main.screenPosition.Y; //j*16 world coords top of block
        //    Texture2D heldItemTexture = TextureAssets.Item[Main.LocalPlayer.HeldItem.type].Value;

        //    Rectangle frame = heldItemTexture.Frame();
        //    spriteBatch.Draw(heldItemTexture, pos, frame, Color.White, 0f, default(Vector2), 0.75f, SpriteEffects.None, 0f);
        //}

        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                CombatText.NewText(Main.LocalPlayer.getRect(), new Color(255, 100, 30, 255), "NOT IN MULTIPLAYER");
            }
            else
            {
                AssWorld.ToggleSlimeRainSky();
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.mouseInterface = true;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemType;
        }
    }
}
