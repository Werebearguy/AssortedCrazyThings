using AssortedCrazyThings.Buffs;
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
		
		/*
			tileFrameCounter[215]++;
			if (tileFrameCounter[215] >= 4)
			{
				tileFrameCounter[215] = 0;
				tileFrame[215]++;
				if (tileFrame[215] >= 8)
				{
					tileFrame[215] = 0;
				}
			}
			num11 = frameY
			if (type == 215)
			{
				frameYOffset = ((num11 >= 36) ? 252 : (tileFrame[type] * 36));
				offsetY = 2;
			}
			
			if (type == 215 && num11 < 36 && rand.Next(3) == 0 && ((drawToScreen && rand.Next(4) == 0) || !drawToScreen) && num11 == 0)
			{
				int num52 = Dust.NewDust(new Vector2((float)(num9 * 16 + 2), (float)(num8 * 16 - 4)), 4, 8, 31, 0f, 0f, 100);
				if (num10 == 0)
				{
					Dust dust7 = Main.dust[num52];
					dust7.position.X = dust7.position.X + (float)rand.Next(8);
				}
				if (num10 == 36)
				{
					Dust dust8 = Main.dust[num52];
					dust8.position.X = dust8.position.X - (float)rand.Next(8);
				}
				Dust dust = Main.dust[num52];
				dust.alpha += rand.Next(100);
				dust = Main.dust[num52];
				dust.velocity *= 0.2f;
				Dust dust9 = Main.dust[num52];
				dust9.velocity.Y = dust9.velocity.Y - (0.5f + (float)rand.Next(10) * 0.1f);
				Main.dust[num52].fadeIn = 0.5f + (float)rand.Next(10) * 0.1f;
			}
			if (type == 215 && num11 < 36)
			{
				int num160 = 15;
				Microsoft.Xna.Framework.Color color14 = new Microsoft.Xna.Framework.Color(255, 255, 255, 0);
				if (num10 / 54 == 5)
				{
					color14 = new Microsoft.Xna.Framework.Color((float)DiscoR / 255f, (float)DiscoG / 255f, (float)DiscoB / 255f, 0f);
				}
				spriteBatch.Draw(FlameTexture[num160], new Vector2((float)(num9 * 16 - (int)screenPosition.X) - ((float)width - 16f) / 2f, (float)(num8 * 16 - (int)screenPosition.Y + offsetY)) + vector, new Microsoft.Xna.Framework.Rectangle(num10, num11 + frameYOffset, width, height), color14, 0f, default(Vector2), 1f, spriteEffects, 0f);
			}
			
			if (Main.tile[myX, myY].type == 215)
			{
				flag2 = true;
				Main.PlaySound(28, myX * 16, myY * 16, 0);
				int num12 = Main.tile[myX, myY].frameX % 54 / 18;
				int num13 = Main.tile[myX, myY].frameY % 36 / 18;
				int num14 = myX - num12;
				int num15 = myY - num13;
				int num16 = 36;
				if (Main.tile[num14, num15].frameY >= 36)
				{
					num16 = -36;
				}
				for (int j = num14; j < num14 + 3; j++)
				{
					for (int k = num15; k < num15 + 2; k++)
					{
						Main.tile[j, k].frameY = (short)(Main.tile[j, k].frameY + num16);
					}
				}
				NetMessage.SendTileSquare(-1, num14 + 1, num15 + 1, 3);
		*/

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
            AddMapEntry(new Color(105, 105, 105));
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
                //Main.LocalPlayer.AddBuff(BuffID.Campfire, 6);
                //Main.LocalPlayer.AddBuff(mod.BuffType<WyvernCampfireBuff>(), 6);
            }
        }

        //public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        //{
        //    Tile tile = Framing.GetTileSafely(i, j);

        //    if (tile.frameY == 0 && !Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
        //    {
        //        int num52 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 - 4)), 4, 8, 31, 0f, 0f, 100);
        //        Dust dust = Main.dust[num52];
        //        dust.alpha += Main.rand.Next(100);
        //        dust = Main.dust[num52];
        //        dust.velocity *= 0.2f;
        //        Dust dust9 = Main.dust[num52];
        //        dust9.velocity.Y = dust9.velocity.Y - (0.5f + (float)Main.rand.Next(10) * 0.1f);
        //        Main.dust[num52].fadeIn = 0.5f + (float)Main.rand.Next(10) * 0.1f;
        //    }
        //}

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
            Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), Lighting.GetColor(i, j), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

            if (tile.frameY == 0 && Main.rand.NextBool() && !Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 - 4)), 4, 8, 31, 0f, 0f, 100)];
                dust.alpha += Main.rand.Next(100);
                dust.velocity *= 0.2f;
                dust.velocity.Y -= (0.5f + (float)Main.rand.Next(10) * 0.1f);
                dust.fadeIn = 0.5f + (float)Main.rand.Next(10) * 0.1f;
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

            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 2; m++)
                {
                    if (Main.tile[l, m] == null)
                    {
                        Main.tile[l, m] = new Tile();
                    }
                    if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
                    {
                        Main.tile[l, m].frameY = (short)(Main.tile[l, m].frameY + change);
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
