using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.DrawLayers
{
    [Content(ContentType.Accessories)]
    public class CrazyBundleOfAssortedBalloonsLayer : AssPlayerLayer
    {
        private Asset<Texture2D> balloonTexture;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                balloonTexture = Mod.Assets.Request<Texture2D>("Items/Accessories/Useful/CrazyBundleOfAssortedBalloons_Balloon_Proper");
            }
        }

        public override void Unload()
        {
            balloonTexture = null;
        }

        public override bool GetDefaultVisiblity(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.balloon == Mod.GetEquipSlot("CrazyBundleOfAssortedBalloons", EquipType.Balloon);
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.BalloonAcc);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            //Since it's supposed to replace the Autoload texture, the regular _Balloon is just blank
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawInfo.shadow != 0f || drawPlayer.dead)
            {
                return;
            }

            int frameY = (Main.hasFocus && (!Main.ingameOptionsWindow || !Main.autoPause)) ? (DateTime.Now.Millisecond % 800 / 200) : 0;
            Vector2 handOffset = Main.OffsetsPlayerOffhand[drawPlayer.bodyFrame.Y / 56];
            if (drawPlayer.direction != 1)
            {
                handOffset.X = drawPlayer.width - handOffset.X;
            }

            if (drawPlayer.gravDir != 1f)
            {
                handOffset.Y -= drawPlayer.height;
            }

            Vector2 offset = new Vector2(0f, 8f) + new Vector2(0f, 6f);
            offset += new Vector2(0f, -drawPlayer.bodyFrame.Height / 3); //Added so it's higher
            Vector2 position = drawInfo.Position - Main.screenPosition + handOffset * new Vector2(1f, drawPlayer.gravDir) + new Vector2(0f, drawPlayer.height - drawPlayer.bodyFrame.Height) + offset;
            position = position.Floor();

            Rectangle frame = balloonTexture.Frame(1, 4, 0, frameY);
            Vector2 origin = new Vector2(26 + drawPlayer.direction * 4, 28f + drawPlayer.gravDir * 6f);
            DrawData data = new DrawData(balloonTexture.Value, position, frame, drawInfo.colorArmorBody, drawPlayer.bodyRotation, origin, 1f, drawInfo.playerEffect, 0)
            {
                shader = drawInfo.cBalloon
            };
            drawInfo.DrawDataCache.Add(data);
        }
    }
}
