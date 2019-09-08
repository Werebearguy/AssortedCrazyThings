using Microsoft.Xna.Framework;
using Terraria;

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

        /// <summary>
        /// Returns the Player that owns the given projectile. Only use if you are certain an owner exists and it is a player
        /// </summary>
        public static Player GetOwner(this Projectile proj)
        {
            return Main.player[proj.owner];
        }
    }
}
