using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;

namespace AssortedCrazyThings
{
    class EverhallowedLanternUI : UIState
    {
        //  Is the UI visible?
        internal static bool visible = false;
        //  Spawn position, i.e. mouse position at the time of pressing the hotkey
        internal static Vector2 spawnPosition;
        internal static Vector2 leftCorner;

        //  Circle diameter
        internal static int mainDiameter = 36;
        //  Circle radius
        internal static int mainRadius = 36 / 2;

        //  Amount of spawned circles
        internal static int circleAmount = -1;
        //  Held item type
        internal static int heldItemType = -1;
        //  List of ammo types matching the properties of the held weapon
        internal static List<int> soulTypes;
        //  Ammo count of ammo types used
        internal static List<bool> soulUnlocked;
        //  Which ammo type is currently highlighted?
        internal static int selectedSoulMinionType = -1;
        //  Which soul type was selected when the UI was opened?
        internal static int currentSoulMinionType = -1;

        //  Initialization
        public override void OnInitialize()
        {
            spawnPosition = new Vector2();
            leftCorner = new Vector2();
            soulTypes = new List<int>();
            soulUnlocked = new List<bool>();
        }

        //  Update ammo type list with any changes made during display of the UI
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Main.NewText("EverhallowedLanternUI Update");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //Main.NewText("EverhallowedLanternUI DrawSelf");
            base.DrawSelf(spriteBatch);

            CompanionDungeonSoulMinionBase.SoulStats stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(currentSoulMinionType);

            //  Draw weapon circle
            Rectangle outputRect = new Rectangle((int)leftCorner.X, (int)leftCorner.Y, mainDiameter, mainDiameter);
            //reset selected type
            bool middle = CheckMouseWithinCircle(Main.MouseScreen, mainRadius, spawnPosition);
            if (middle)
            {
                selectedSoulMinionType = -1;
            }

            spriteBatch.Draw(Main.wireUITexture[middle ? 1 : 0], outputRect, Color.White);

            //  Draw weapon inside circle
            if (heldItemType != -1)
            {
                int finalWidth = Main.itemTexture[heldItemType].Width / 2,
                    finalHeight = Main.itemTexture[heldItemType].Height / 2;
                Rectangle outputWeaponRect = new Rectangle((int)spawnPosition.X - (finalWidth / 2), (int)spawnPosition.Y - (finalHeight / 2), finalWidth, finalHeight);
                spriteBatch.Draw(Main.itemTexture[heldItemType], outputWeaponRect, Color.White);
            }

            bool[] unlocked = new bool[]
            {
                true,                //      0
                NPC.downedMechBoss3, //skele 1
                NPC.downedMechBoss2, //twins 2
                NPC.downedMechBoss1, //destr 3
            };

            int outerRadius = 48;
            //  Apply offset depending on the amount of circles to be drawn
            double offset = 0.25;
            //  Angle between each circle
            //  (*Math.PI is in isMouseWithin)
            double angleSteps = 2.0d / circleAmount; //set to 4

            int soulType = 0;
            //  Starting angle
            double i = offset;
            //  done --> ID of currently drawn circle
            for (soulType = 0; soulType < circleAmount; ++soulType)
            {
                stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(soulType);

                double x = outerRadius * Math.Cos(i * Math.PI);
                double y = outerRadius * Math.Sin(i * Math.PI);

                Rectangle ammoBgRect = new Rectangle((int)(leftCorner.X + x), (int)(leftCorner.Y + y), mainDiameter, mainDiameter);
                //  Check if mouse is within the circle checked
                //bool isMouseWithin = CheckMouseWithinCircle(Main.MouseScreen, mainRadius + 8, new Vector2((int)(spawnPosition.X + x), (int)(spawnPosition.Y + y)));
                bool isMouseWithin = CheckMouseWithinWheel(Main.MouseScreen, spawnPosition, 96, mainRadius, offset * Math.PI, circleAmount, soulType);

                //  Actually draw the bg circle
                Color drawColor = Color.White;
                if (!unlocked[soulType]) drawColor = Color.DarkRed;
                spriteBatch.Draw(Main.wireUITexture[isMouseWithin ? 1 : 0], ammoBgRect, (soulType == currentSoulMinionType) ? Color.Gray : drawColor);

                //  Draw ammo sprites over the icons
                int projWidth = Main.projectileTexture[stats.Type].Width;
                int projHeight = Main.projectileTexture[stats.Type].Height / Main.projFrames[stats.Type];
                Rectangle projRect = new Rectangle((int)(spawnPosition.X + x) - (projWidth / 2), (int)(spawnPosition.Y + y) - (projHeight / 2), projWidth, projHeight);

                Rectangle sourceRect = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = projWidth,
                    Height = projHeight
                };

                drawColor = Color.White;
                if (!unlocked[soulType]) drawColor = Color.Gray;
                spriteBatch.Draw(Main.projectileTexture[stats.Type], projRect, sourceRect, (soulType == currentSoulMinionType) ? Color.Gray : drawColor);

                if (isMouseWithin && unlocked[soulType])
                {
                    //set the "returned" new type
                    selectedSoulMinionType = soulType;

                    //  Draw the tooltip
                    Color fontColor = Color.White;
                    Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
                    CompanionDungeonSoulMinionBase.SoulType tempSoulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
                    string tooltip = tempSoulType.ToString()
                        + "\nBase Damage: " + stats.Damage
                        + "\nBase Knockback: " + stats.Knockback;
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, tooltip, mousePos + new Vector2(15, 15), fontColor, 0, Vector2.Zero, Vector2.One);
                }

                i += angleSteps;
            }

            //render current soul tooltip
            if (middle)
            {
                Color fontColor = Color.White;
                Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
                CompanionDungeonSoulMinionBase.SoulType tempSoulType = (CompanionDungeonSoulMinionBase.SoulType)currentSoulMinionType;
                string tooltip = "Current: " + tempSoulType.ToString();
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, tooltip, mousePos + new Vector2(15, 15), fontColor, 0, Vector2.Zero, Vector2.One);
            }
        }

        internal bool CheckMouseWithinCircle(Vector2 mousePos, int radius, Vector2 center)
        {
            return ((mousePos.X - center.X) * (mousePos.X - center.X) + (mousePos.Y - center.Y) * (mousePos.Y - center.Y) <= radius * radius);
        }

        internal bool CheckMouseWithinWheel(Vector2 mousePos, Vector2 center, int outerRadius, int innerRadius, double offset, int pieceCount, int elementNumber)
        {
            //  Check if mouse cursor is inside the outer circle
            bool first = ((mousePos.X - center.X) * (mousePos.X - center.X) + (mousePos.Y - center.Y) * (mousePos.Y - center.Y) <= outerRadius * outerRadius);
            //  Check if mouse cursor is outside the inner circle
            bool second = ((mousePos.X - center.X) * (mousePos.X - center.X) + (mousePos.Y - center.Y) * (mousePos.Y - center.Y) > innerRadius * innerRadius);


            double finalOffset = offset;
            finalOffset *= 180 / Math.PI;
            if (pieceCount == 2) finalOffset -= 90;
            if (pieceCount == 3) finalOffset -= 60;
            if (pieceCount == 4) finalOffset -= 45;
            if (pieceCount == 5) finalOffset -= 35;

            double step = 360 / pieceCount;
            double beginAngle = (finalOffset + step * (elementNumber)) % 360, endAngle = (beginAngle + step) % 360;
            if (beginAngle < 0) beginAngle = 360 + beginAngle;

            //  Calculate x,y coords on outer circle
            double calculatedAngle = Math.Atan2(mousePos.Y - center.Y, mousePos.X - center.X);
            calculatedAngle = calculatedAngle * 180 / Math.PI;

            if (calculatedAngle < 0)
            {
                calculatedAngle = 360 + calculatedAngle;
            }

            double calculatedDistance = Math.Sqrt((mousePos.X - center.X) * (mousePos.X - center.X) + (mousePos.Y - center.Y) * (mousePos.Y - center.Y));

            bool third = false;
            //(calculatedAngle <= endAngle && calculatedAngle >= beginAngle);
            if (beginAngle < endAngle)
            {
                if (beginAngle < calculatedAngle && calculatedAngle < endAngle)
                {
                    third = true;
                }
            }
            else
            {
                if (calculatedAngle > beginAngle && calculatedAngle > endAngle)
                {
                    third = true;
                }
                if (calculatedAngle < beginAngle && calculatedAngle < endAngle)
                {
                    third = true;
                }

            }




            //Main.NewText(beginAngle + " " + endAngle + " " + calculatedAngle);
            bool fourth = calculatedDistance <= outerRadius;

            //Main.NewText(first + " " + second + " " + third + " " + fourth);
            return first && second && third && fourth;
        }
    }
}