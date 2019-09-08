using AssortedCrazyThings.NPCs.CuteSlimes;
using Microsoft.Xna.Framework;

namespace AssortedCrazyThings.Base
{
    /// <summary>
    /// contains Extensions for some basic tasks
    /// </summary>
    public static class AssExtensions
    {
        /// <summary>
        /// Returns the average value between the RGB (ignoring A)
        /// </summary>
        public static float GetAverage(this Color color)
        {
            return (color.R + color.G + color.B) / 3f;
        }
    }
}
