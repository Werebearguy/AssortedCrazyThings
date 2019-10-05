using AssortedCrazyThings.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Slime Beacon");
            AddMapEntry(new Color(75, 139, 166), name);
            dustType = 1;
            animationFrameHeight = 56;
            disableSmartCursor = true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType<SlimeBeaconItem>());
            AssWorld.DisableSlimeRainSky();
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

        public override bool NewRightClick(int i, int j)
        {
            Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
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
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType<SlimeBeaconItem>();
        }
    }
}
