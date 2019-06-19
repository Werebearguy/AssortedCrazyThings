using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AssortedCrazyThings.UI
{
    //Huge credit to Muzuwi (with permission): https://github.com/Muzuwi/AmmoboxPlus/blob/master/AmmoboxUI.cs
    /// <summary>
    /// UI that is used to select something out of a list of things, opened via item
    /// </summary>
    public class CircleUI : UIState
	{
		internal const int NONE = -1;

		/// <summary>
		/// Circle diameter
		/// </summary>
		internal const int mainDiameter = 36;

		/// <summary>
		/// Circle radius
		/// </summary>
		internal const int mainRadius = mainDiameter / 2;

		/// <summary>
		/// Is the UI visible?
		/// </summary>
		internal static bool visible = false;

        /// <summary>
        /// Spawn position, i.e. mouse position at UI start
        /// </summary>
        internal static Vector2 spawnPosition = default(Vector2);

        /// <summary>
        /// Held item type
        /// </summary>
        internal static int heldItemType = -1;

        /// <summary>
        /// Which thing is currently highlighted?
        /// </summary>
        internal static int returned = NONE;

        /// <summary>
        /// Which thing was the previously selected one?
        /// </summary>
        internal static int currentSelected = NONE;

        /// <summary>
        /// Which button was it activated with
        /// </summary>
        internal static bool openedWithLeft = false;

        /// <summary>
        /// Fade in animation when opening the UI
        /// </summary>
        internal static float fadeIn = 0;

        /// <summary>
        /// Holds data about what to draw
        /// </summary>
        internal static CircleUIConf UIConf;

        /// <summary>
        /// Spawn position offset to top left corner of that to draw the icons
        /// </summary>
        private Vector2 TopLeftCorner
        {
            get
            {
                return spawnPosition - new Vector2(mainRadius, mainRadius);
            }
        }

        //TODO add some static var that has the current segment the mouse is in

        //Update, unused
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //Draw
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Main.LocalPlayer.mouseInterface = true;

            //48
            int outerRadius = 48;
            if (UIConf.CircleAmount > 5) outerRadius += 5 * (UIConf.CircleAmount - 5); //increase by 5 after having more than 5 options, starts getting clumped at about 24 circles
            if (fadeIn < outerRadius) outerRadius = (int)(fadeIn += (float)outerRadius / 10);

            double angleSteps = 2.0d / UIConf.CircleAmount;
            int done;
            //done --> ID of currently drawn circle
            for (done = 0; done < UIConf.CircleAmount; done++)
            {
                double x = outerRadius * Math.Sin(angleSteps * done * Math.PI);
                double y = outerRadius * -Math.Cos(angleSteps * done * Math.PI);
                
                Rectangle bgRect = new Rectangle((int)(TopLeftCorner.X + x), (int)(TopLeftCorner.Y + y), mainDiameter, mainDiameter);
                //Check if mouse is within the circle checked
                bool isMouseWithin = CheckMouseWithinWheel(Main.MouseScreen, spawnPosition, mainRadius, UIConf.CircleAmount, done);

                //Actually draw the bg circle
                Color drawColor = Color.White;
                if (!UIConf.Unlocked[done])
                {
                    drawColor = Color.DarkRed;
                }
                else if (done == currentSelected)
                {
                    drawColor = Color.Gray;
                }
                spriteBatch.Draw(Main.wireUITexture[isMouseWithin ? 1 : 0], bgRect, drawColor);

                //Draw sprites over the icons
                int width = UIConf.Textures[done].Width;
                int height = UIConf.Textures[done].Height;
                if (UIConf.SpritesheetDivider > 0) height /= UIConf.SpritesheetDivider;
                Rectangle projRect = new Rectangle((int)(spawnPosition.X + x) - (width / 2), (int)(spawnPosition.Y + y) - (height / 2), width, height);

                drawColor = Color.White;
                if (done == currentSelected || !UIConf.Unlocked[done]) drawColor = Color.Gray;

                Rectangle sourceRect = new Rectangle(0, 0, width, height);

                spriteBatch.Draw(UIConf.Textures[done], projRect, sourceRect, drawColor);

                if (isMouseWithin)
                {
                    if (UIConf.Unlocked[done])
                    {
                        //set the "returned" new type
                        returned = done;
                    }
                    else
                    {
                        //if hovering over a locked thing, don't return anything new
                        returned = currentSelected;
                    }
                }
            }

            Texture2D bgTexture = Main.wireUITexture[0];

            //Draw held item bg circle
            Rectangle outputRect = new Rectangle((int)TopLeftCorner.X, (int)TopLeftCorner.Y, mainDiameter, mainDiameter);

            bool middle = CheckMouseWithinCircle(Main.MouseScreen, mainRadius, spawnPosition);

            spriteBatch.Draw(Main.wireUITexture[middle ? 1 : 0], outputRect, Color.White);

            //Draw held item inside circle
            if (heldItemType != -1)
            {
                int finalWidth = Main.itemTexture[heldItemType].Width/* / 2*/;
                int finalHeight = Main.itemTexture[heldItemType].Height/* / 2*/;
                Rectangle outputItemRect = new Rectangle((int)spawnPosition.X - (finalWidth / 2), (int)spawnPosition.Y - (finalHeight / 2), finalWidth, finalHeight);
                //outputWeaponRect.Inflate(4, 4);
                spriteBatch.Draw(Main.itemTexture[heldItemType], outputItemRect, Color.White);
            }

            if (middle)
            {
                //if hovering over the middle, don't return anything new
                returned = currentSelected;
            }

            //extra loop so tooltips are always drawn after the circles
            for (done = 0; done < UIConf.CircleAmount; done++)
            {
                bool isMouseWithin = CheckMouseWithinWheel(Main.MouseScreen, spawnPosition, mainRadius, UIConf.CircleAmount, done);
                string tooltip = UIConf.Unlocked[done] ? UIConf.Tooltips[done] : UIConf.ToUnlock[done];

                //if there is a "to unlock" message, prefix it
                tooltip = (!UIConf.Unlocked[done] && UIConf.ToUnlock[done] != "") ? ("To unlock: " + tooltip) : tooltip;

                if (isMouseWithin)
                {
                    //Draw the tooltip
                    Color fontColor = Color.White;
                    Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, tooltip, mousePos + new Vector2(16, 16), fontColor, 0, Vector2.Zero, Vector2.One);
                }
            }
        }

        /// <summary>
        /// Check if the mouse cursor is within the radius around the position specified by center
        /// </summary>
        internal static bool CheckMouseWithinCircle(Vector2 mousePos, int radius, Vector2 center)
        {
            return ((mousePos.X - center.X) * (mousePos.X - center.X) + (mousePos.Y - center.Y) * (mousePos.Y - center.Y)) <= radius * radius;
        }

        /// <summary>
        /// Checks if the mouse cursor is currently inside the segment specified by the arguments. Decided by angle (radius only matters for the inner element).
        /// </summary>
        internal static bool CheckMouseWithinWheel(Vector2 mousePos, Vector2 center, int innerRadius, int pieceCount, int elementNumber)
        {
            //Check if mouse cursor is outside the inner circle
            bool outsideInner = ((mousePos.X - center.X) * (mousePos.X - center.X) + (mousePos.Y - center.Y) * (mousePos.Y - center.Y)) > innerRadius * innerRadius;

            double step = 360 / pieceCount;
            //finalOffset *= 180 / Math.PI;
            double finalOffset = -step / 2;

            double beginAngle = (finalOffset + step * elementNumber) % 360;
            double endAngle = (beginAngle + step) % 360;
            if (beginAngle < 0) beginAngle = 360 + beginAngle;

            //Calculate x,y coords on outer circle
            double calculatedAngle = Math.Atan2(mousePos.X - center.X, - (mousePos.Y - center.Y));
            calculatedAngle = calculatedAngle * 180 / Math.PI;

            if (calculatedAngle < 0)
            {
                calculatedAngle = 360 + calculatedAngle;
            }

            bool insideSegment = false;
            //(calculatedAngle <= endAngle && calculatedAngle >= beginAngle);
            if (beginAngle < endAngle)
            {
                if (beginAngle < calculatedAngle && calculatedAngle < endAngle)
                {
                    insideSegment = true;
                }
            }
            else
            {
                if (calculatedAngle > beginAngle && calculatedAngle > endAngle)
                {
                    insideSegment = true;
                }
                if (calculatedAngle < beginAngle && calculatedAngle < endAngle)
                {
                    insideSegment = true;
                }
            }

            return outsideInner && insideSegment;
        }

        /// <summary>
        /// Called when the UI is about to appear
        /// </summary>
        public static void Start(int triggerType, bool triggerLeft)
        {
            visible = true;
            spawnPosition = Main.MouseScreen;
            heldItemType = triggerType;
            openedWithLeft = triggerLeft;
            fadeIn = 0;
        }
    }
}
