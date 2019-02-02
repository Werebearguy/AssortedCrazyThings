using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedCrazyThings.Tiles
{
    class SlimeBeaconTile : ModTile
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
            AddMapEntry(new Color(75, 139, 166));
            dustType = 1;
            animationFrameHeight = 56;
            disableSmartCursor = true;
            //adjTiles = new int[] { TileID.LunarMonolith };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType<SlimeBeaconItem>());
            AssWorld.DisableSlimeRainSky();
        }

        //public override void NearbyEffects(int i, int j, bool closer)
        //{
        //    if (Main.tile[i, j].frameY >= 56)
        //    {
        //        //ExamplePlayer modPlayer = Main.LocalPlayer.GetModPlayer<ExamplePlayer>(mod);
        //        //modPlayer.voidMonolith = true;
        //    }
        //}

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

        public override void RightClick(int i, int j)
        {
            Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y, Main.LocalPlayer.width, Main.LocalPlayer.height), new Color(255, 100, 30, 255), "NOT IN MULTIPLAYER");
            }
            else
            {
                AssWorld.ToggleSlimeRainSky();
            }
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.showItemIcon = true;
            Main.LocalPlayer.showItemIcon2 = mod.ItemType<SlimeBeaconItem>();
        }
    }
}
