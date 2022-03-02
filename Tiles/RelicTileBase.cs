using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using System;
using ReLogic.Content;
using Terraria.Localization;

namespace AssortedCrazyThings.Tiles
{
    [Content(ContentType.Bosses)] //Relic on a non-boss is not a thing
    public abstract class RelicTileBase<T> : DroppableTile<T> where T : ModItem
    {
        //Every relic has its own extra floating part, should be 50x50
        public abstract string Extra { get; }

        //All relics use the same pedestal texture
        public override string Texture => "AssortedCrazyThings/Tiles/RelicTileBase_ACT";

        public Asset<Texture2D> ExtraAsset;

        public override void Unload()
        {
            //Unload the special texture displayed on the pedestal
            ExtraAsset = null;
        }

        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                //Cache the special texture displayed on the pedestal
                ExtraAsset = ModContent.Request<Texture2D>(Extra);
            }

            Main.tileShine[Type] = 400; //Responsible for golden particles
            Main.tileFrameImportant[Type] = true; //Any multitile requires this
            TileID.Sets.InteractibleByNPCs[Type] = true; //Town NPCs will behave differently when this tile is nearby

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4); //Relics are 3x4
            TileObjectData.newTile.LavaDeath = false; //Does not break when lava touches it
            TileObjectData.newTile.DrawYOffset = 2; //So the tile sinks into the ground
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft; //Player faces to the left
            TileObjectData.newTile.StyleHorizontal = false; //Based on how the alternate sprites are positioned on the sprite (by default, true)

            //If you decide to make your tile the same way vanilla does (one tile, different place styles), you need these, aswell as the code in SetDrawPositions
            //TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
            //TileObjectData.newTile.StyleMultiplier = 2;
            //TileObjectData.newTile.StyleWrapLimit = 2;
            //TileObjectData.newTile.styleLineSkipVisualOverride = 0;

            //Register an alternate tile data with flipped direction
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile); //Copy everything from above, saves us some code
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //Player faces to the right
            TileObjectData.addAlternate(1);

            //Register the tile data itself
            TileObjectData.addTile(Type);

            //Register map name and color
            //"MapObject.Relic" refers to the translation key for the vanilla "Relic" text
            AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemType);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        //public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        //{
        //    //Only required If you decide to make your tile the same way vanilla does (one tile, different place styles), you need this

        //    //This preserves its original frameX/Y which is required for determining the correct texture floating on the pedestal, but makes it draw properly
        //    tileFrameX %= 18 * 3; //Clamps the frameX
        //    tileFrameY %= 18 * 4 * 2; //Clamps the frameY (two horizontal aligned place styles, hence * 2)
        //}

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            //Since this tile does not have the hovering part on its sheet, we have to animate it ourselves
            //Therefore we register the top-left of the tile as a "special point"
            //This allows us to draw things in SpecialDraw
            if (drawData.tileFrameX % (18 * 3) == 0 && drawData.tileFrameY % (18 * 4) == 0)
            {
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
            }
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //This is lighting-mode specific, always include this if you draw tiles manually
            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
            {
                offScreen = Vector2.Zero;
            }

            //Take the tile, check if it actually exists and is ours
            Point p = new Point(i, j);
            Tile tile = Main.tile[p.X, p.Y];
            if (tile != null && tile.HasTile && tile.TileFrameX % (18 * 3) == 0 && tile.TileFrameX % (18 * 4) == 0)
            {
                //Get the initial draw parameters
                Texture2D texture = ExtraAsset.Value;

                int frameY = tile.TileFrameX / (18 * 3);
                int horizontalFrames = 1;
                int verticalFrames = 1; //Increase this number to match the amount of frames you have on your extra sheet
                Rectangle frame = texture.Frame(horizontalFrames, verticalFrames, 0, frameY);

                Vector2 origin = frame.Size() / 2f;
                Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

                Color color = Lighting.GetColor(p.X, p.Y);

                bool direction = tile.TileFrameY / (18 * 4) != 0; //This is related to the alternate tile data we registered before
                SpriteEffects effects = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                //Some math magic to make it smoothly move up and down over time
                const float TwoPi = (float)Math.PI * 2f;
                float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 5f);
                Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -40f) + new Vector2(0f, offset * 4f);

                //Draw the main texture
                spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1f, effects, 0f);

                //Draw the periodic glow effect
                float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 0.7f;
                Color effectColor = color;
                effectColor.A = 0;
                effectColor = effectColor * 0.1f * scale;
                for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
                {
                    spriteBatch.Draw(texture, drawPos + (TwoPi * num5).ToRotationVector2() * (6f + offset * 2f), frame, effectColor, 0f, origin, 1f, effects, 0f);
                }
            }
        }
    }
}
