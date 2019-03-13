using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AssortedCrazyThings
{
    class AssUtils
    {
        public static AssortedCrazyThings Instance { get; set; } //just shorter writing AssUtils.Instance than AssortedCrazyThings.Instance

        public static int[] isModdedWormBodyOrTail;

        public static void Print(object o)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                Console.WriteLine(o.ToString());
            }

            if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(o.ToString());
            }
        }

        public static Dust QuickDust(int dustType, Vector2 pos, Color color, Vector2 dustVelo = default(Vector2), int alpha = 0, float scale = 1f)
        {
            Dust dust = Dust.NewDustPerfect(pos, dustType, dustVelo, alpha, color, scale);
            dust.position = pos;
            dust.velocity = dustVelo;
            dust.fadeIn = 1f;
            dust.noLight = false;
            dust.noGravity = true;
            return dust;
        }

        public static void QuickDustLine(int dustType, Vector2 start, Vector2 end, float splits, Color color, Vector2 dustVelo = default(Vector2), int alpha = 0, float scale = 1f)
        {
            QuickDust(dustType, start, color, dustVelo);
            float num = 1f / splits;
            for (float num2 = 0f; num2 < 1f; num2 += num)
            {
                QuickDust(dustType, Vector2.Lerp(start, end, num2), color, dustVelo, alpha, scale);
            }
        }

        public static Dust DrawDustAtPos(int dustType, Vector2 pos)
        {
            //used for showing a position as a dust on the screen for debugging
            Dust dust = QuickDust(dustType, pos, Color.White);
            dust.noGravity = true;
            dust.noLight = true;
            return dust;
        }

        public static bool AnyNPCs(int[] types)
        {
            //Like AnyNPCs but checks for an array
            for (int i = 0; i < types.Length; i++)
            {
                if (NPC.AnyNPCs(types[i])) return true;
            }
            return false;
        }

        public static bool AnyNPCs(List<int> types)
        {
            int[] typesArray = types.ToArray();
            //Like AnyNPCs but checks for an array
            for (int i = 0; i < typesArray.Length; i++)
            {
                if (NPC.AnyNPCs(typesArray[i])) return true;
            }
            return false;
        }

        public static bool IsWormBodyOrTail(NPC npc)
        {
            return Array.BinarySearch(isModdedWormBodyOrTail, npc.type) >= 0 || npc.dontCountMe;
        }

        public static string GetTimeAsString(bool accurate = true)
        {
            string text3 = Lang.inter[95].Value;
            string text4 = "AM";
            double num6 = Main.time;
            if (!Main.dayTime)
            {
                num6 += 54000.0;
            }
            num6 = num6 / 86400.0 * 24.0;
            double num7 = 7.5;
            num6 = num6 - num7 - 12.0;
            if (num6 < 0.0)
            {
                num6 += 24.0;
            }
            if (num6 >= 12.0)
            {
                text4 = "PM";
            }
            int num8 = (int)num6;
            double num9 = num6 - (double)num8;
            num9 = (double)(int)(num9 * 60.0);
            string text5 = string.Concat(num9);
            if (num9 < 10.0)
            {
                text5 = "0" + text5;
            }
            if (num8 > 12)
            {
                num8 -= 12;
            }
            if (num8 == 0)
            {
                num8 = 12;
            }
            if (!accurate) text5 = ((!(num9 < 30.0)) ? "30" : "00");
            return text3 + ": " + num8 + ":" + text5 + " " + text4;
        }

        public static string GetMoonPhaseAsString(bool showNumber = false)
        {
            string suffix = "";
            if (showNumber) suffix = " (" + (Main.moonPhase + 1) + ")";
            string text3 = Lang.inter[102].Value;
            if (Main.moonPhase == 0)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.FullMoon") + suffix;
            }
            else if (Main.moonPhase == 1)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaningGibbous") + suffix;
            }
            else if (Main.moonPhase == 2)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.ThirdQuarter") + suffix;
            }
            else if (Main.moonPhase == 3)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaningCrescent") + suffix;
            }
            else if (Main.moonPhase == 4)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.NewMoon") + suffix;
            }
            else if (Main.moonPhase == 5)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaxingCrescent") + suffix;
            }
            else if (Main.moonPhase == 6)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.FirstQuarter") + suffix;
            }
            else if (Main.moonPhase == 7)
            {
                return text3 + ": " + Language.GetTextValue("GameUI.WaxingGibbous") + suffix;
            }
            return "";
        }
    }
}
