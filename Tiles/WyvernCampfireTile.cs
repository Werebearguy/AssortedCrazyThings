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

namespace AssortedCrazyThings.Tiles
{
    class WyvernCampfireTile : ModTile
    {
        private const int maxFrames = 8;

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Wyvern Campfire");
            AddMapEntry(new Color(105, 105, 105), name);
            dustType = -1;
            animationFrameHeight = 36;
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Campfire };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType<WyvernCampfireItem>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.frameY == 0)
            {
                r = 0.5f;
                g = 0.7f;
                b = 1.2f;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer && Main.tile[i, j].frameY < animationFrameHeight)
            {
                Main.LocalPlayer.GetModPlayer<AssPlayer>().wyvernCampfire = true;
                Main.campfire = true;
            }
        }

        //you need these four things for the outline to work:
        //_Highlight.png
        //TileID.Sets.HasOutlines[Type] = true;
        //disableSmartCursor = true;
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

        //for the change in HitWire to actually register
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Texture2D texture;
            if (Main.canDrawColorTile(i, j))
            {
                texture = Main.tileAltTexture[Type, (int)tile.color()];
            }
            else
            {
                texture = Main.tileTexture[Type];
            }
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            //int height = 16;
            int height = tile.frameY == 18 ? 18 : 16;
            int animate = animationFrameHeight * (maxFrames - 1);
            if (tile.frameY < animationFrameHeight)
            {
                animate = Main.tileFrame[Type] * animationFrameHeight;
            }
            Color color = Lighting.GetColor(i, j);
            Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            pos.Y += 2;
            Rectangle frame = new Rectangle(tile.frameX, tile.frameY + animate, 16, height);
            Main.spriteBatch.Draw(texture, pos, frame, color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

            texture = null;
            Color transparent = Color.Transparent;
            if (TileID.Sets.HasOutlines[Type] && Collision.InTileBounds(i, j, Main.TileInteractionLX, Main.TileInteractionLY, Main.TileInteractionHX, Main.TileInteractionHY) && Main.SmartInteractTileCoords.Contains(new Point(i, j)))
            {
                int average = (int)color.GetAverage();
                bool selected = false;
                if (Main.SmartInteractTileCoordsSelected.Contains(new Point(i, j)))
                {
                    selected = true;
                }
                if (average > 10)
                {
                    texture = Main.highlightMaskTexture[Type];
                    if (selected)
                    {
                        transparent = new Color(average, average, average / 3, average);
                    }
                    else
                    {
                        transparent = new Color(average / 2, average / 2, average / 2, average);
                    }
                }
                if (texture != null) Main.spriteBatch.Draw(texture, pos, frame, transparent, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

            }

            if (tile.frameY == 0 && Main.rand.NextBool() && !Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
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
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.showItemIcon = true;
            Main.LocalPlayer.showItemIcon2 = mod.ItemType<WyvernCampfireItem>();
        }

        public override void RightClick(int i, int j)
        {
            Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
            HitWire(i, j);
        }

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 3;
            int y = j - Main.tile[i, j].frameY / 18 % 2;
            int change = animationFrameHeight;
            if (Main.tile[x, y].frameY >= animationFrameHeight)
            {
                change = -animationFrameHeight;
            }

            Tile tile;
            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 2; m++)
                {
                    tile = Main.tile[l, m];
                    if (tile == null)
                    {
                        tile = new Tile();
                    }
                    if (tile.active() && tile.type == Type)
                    {
                        tile.frameY = (short)(tile.frameY + change);
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
