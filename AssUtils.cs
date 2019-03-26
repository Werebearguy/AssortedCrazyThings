using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AssortedCrazyThings
{
    static class AssUtils
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

        public static Dust DrawDustAtPos(Vector2 pos, int dustType = 169)
        {
            //used for showing a position as a dust for debugging
            Dust dust = QuickDust(dustType, pos, Color.White);
            dust.noGravity = true;
            dust.noLight = true;
            return dust;
        }

        public static T[] ConcatArray<T>(T[] first, T[] second)
        {
            T[] combined = new T[first.Length + second.Length];
            Array.Copy(first, combined, first.Length);
            Array.Copy(second, 0, combined, first.Length, second.Length);
            return combined;
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
            return Array.BinarySearch(isModdedWormBodyOrTail, npc.type) >= 0 || npc.dontCountMe || npc.realLife != -1;
        }

        public static string GetTimeAsString(bool accurate = true)
        {
            string suffix = "AM";
            double doubletime = Main.time;
            if (!Main.dayTime)
            {
                doubletime += 54000.0;
            }
            doubletime = doubletime / 86400.0 * 24.0;
            double wtf = 7.5;
            doubletime = doubletime - wtf - 12.0;
            if (doubletime < 0.0)
            {
                doubletime += 24.0;
            }
            if (doubletime >= 12.0)
            {
                suffix = "PM";
            }
            int hours = (int)doubletime;
            double doubleminutes = doubletime - hours;
            doubleminutes = (int)(doubleminutes * 60.0);
            string minutes = string.Concat(doubleminutes);
            if (doubleminutes < 10.0)
            {
                minutes = "0" + minutes;
            }
            if (hours > 12)
            {
                hours -= 12;
            }
            if (hours == 0)
            {
                hours = 12;
            }
            if (!accurate) minutes = (!(doubleminutes < 30.0)) ? "30" : "00";
            return Language.GetTextValue("Game.Time", hours + ":" + minutes + " " + suffix);
        }

        public static string GetMoonPhaseAsString(bool showNumber = false)
        {
            string suffix = "";
            if (showNumber) suffix = " (" + (Main.moonPhase + 1) + ")";
            string prefix = Lang.inter[102].Value + ": "; //can't seem to find "Moon Phase" in the lang files for GameUI
            if (Main.moonPhase == 0)
            {
                return prefix + Language.GetTextValue("GameUI.FullMoon") + suffix;
            }
            else if (Main.moonPhase == 1)
            {
                return prefix + Language.GetTextValue("GameUI.WaningGibbous") + suffix;
            }
            else if (Main.moonPhase == 2)
            {
                return prefix + Language.GetTextValue("GameUI.ThirdQuarter") + suffix;
            }
            else if (Main.moonPhase == 3)
            {
                return prefix + Language.GetTextValue("GameUI.WaningCrescent") + suffix;
            }
            else if (Main.moonPhase == 4)
            {
                return prefix + Language.GetTextValue("GameUI.NewMoon") + suffix;
            }
            else if (Main.moonPhase == 5)
            {
                return prefix + Language.GetTextValue("GameUI.WaxingCrescent") + suffix;
            }
            else if (Main.moonPhase == 6)
            {
                return prefix + Language.GetTextValue("GameUI.FirstQuarter") + suffix;
            }
            else if (Main.moonPhase == 7)
            {
                return prefix + Language.GetTextValue("GameUI.WaxingGibbous") + suffix;
            }
            return "";
        }
    }
}
