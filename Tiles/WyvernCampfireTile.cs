using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using Terraria.GameContent;

namespace AssortedCrazyThings.Tiles
{
    [Content(ContentType.Placeables)]
    public class WyvernCampfireTile : DroppableTile<WyvernCampfireItem>
    {
        private const int maxFrames = 8;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wyvern Campfire");
            AddMapEntry(new Color(105, 105, 105), name);
            DustType = -1;
            AnimationFrameHeight = 36;
            AdjTiles = new int[] { TileID.Campfire };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemType);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0)
            {
                r = 0.54f;
                g = 0.76f;
                b = 1.3f;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer && Main.tile[i, j].TileFrameY < AnimationFrameHeight)
            {
                Main.LocalPlayer.GetModPlayer<AssPlayer>().wyvernCampfire = true;
                Main.SceneMetrics.HasCampfire = true;
            }
        }

        //you need these four things for the outline to work:
        //_Highlight.png
        //TileID.Sets.HasOutlines[Type] = true;
        //TileID.Sets.DisableSmartCursor[Type] = true;
        //and this hook
        public override bool HasSmartInteract()
        {
            return true;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.Campfire];
            frameCounter = Main.tileFrameCounter[TileID.Campfire];
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = Main.instance.TilesRenderer.GetTileDrawTexture(tile, i, j);
            Vector2 zero = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            //int height = 16;
            int height = tile.TileFrameY == 18 ? 18 : 16;
            int animate = AnimationFrameHeight * (maxFrames - 1);
            if (tile.TileFrameY < AnimationFrameHeight)
            {
                animate = Main.tileFrame[Type] * AnimationFrameHeight;
            }
            Color color = Lighting.GetColor(i, j);
            Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            pos.Y += 2;
            Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY + animate, 16, height);
            spriteBatch.Draw(texture, pos, frame, color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

            AssUtils.DrawTileHighlight(spriteBatch, i, j, Type, color, pos, frame);

            if (tile.TileFrameY == 0 && Main.rand.NextBool() && !Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(i * 16 + 2, j * 16 - 4), 4, 8, 31, 0f, 0f, 100);
                dust.alpha += Main.rand.Next(100);
                dust.velocity *= 0.2f;
                dust.velocity.Y -= 0.5f + Main.rand.Next(10) * 0.1f;
                dust.fadeIn = 0.5f + Main.rand.Next(10) * 0.1f;
            }

            return false;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.mouseInterface = true;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemType;
        }

        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
            HitWire(i, j);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 3;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
            int change = AnimationFrameHeight;
            if (Main.tile[x, y].TileFrameY >= AnimationFrameHeight)
            {
                change = -AnimationFrameHeight;
            }

            Tile tile;
            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 2; m++)
                {
                    tile = Framing.GetTileSafely(l, m);
                    if (tile.HasTile && tile.TileType == Type)
                    {
                        tile.TileFrameY = (short)(tile.TileFrameY + change);
                    }
                }
            }

            if (Wiring.running)
            {
                Wiring.SkipWire(x, y);
                Wiring.SkipWire(x, y + 1);
                Wiring.SkipWire(x + 1, y);
                Wiring.SkipWire(x + 1, y + 1);
                Wiring.SkipWire(x + 2, y);
                Wiring.SkipWire(x + 2, y + 1);
            }
            NetMessage.SendTileSquare(-1, x + 1, y, 3);
        }
    }
}
