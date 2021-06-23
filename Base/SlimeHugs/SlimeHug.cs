using AssortedCrazyThings.Projectiles.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SlimeHugs
{
    [Content(ContentType.CuteSlimes)]
    [Autoload(false)]
    public abstract class SlimeHug : ModType, ICloneable, IComparable<SlimeHug>
    {
        public virtual int HugDuration => 120;

        public virtual int HugEmote => EmoteID.EmotionLove;

        public virtual int HugEmoteDuration => 120;

        public virtual int PreHugEmote => -1;

        public virtual int PreHugEmoteDuration => 60;

        protected virtual int Cooldown => 60 * 60 * 10; //10 minutes

        public int Type { get; internal set; }

        public int CooldownTimer { get; private set; }

        protected override void Register()
        {
            Type = SlimeHugLoader.Register(this);
        }

        /// <summary>
        /// Brings the hug closer to triggering. Returns true if ready
        /// </summary>
        internal bool HandleCooldown()
        {
            if (CooldownTimer < Cooldown)
            {
                CooldownTimer++;
            }
            return CooldownTimer == Cooldown;
        }

        /// <summary>
        /// Resets the cooldown
        /// </summary>
        internal void ApplyCooldown()
        {
            CooldownTimer = 0;
        }

        /// <summary>
        /// Allows customizing under which conditions this (ready) hug should be applied
        /// </summary>
        public virtual bool IsAvailable(CuteSlimeBaseProj slime, PetPlayer petPlayer)
        {
            return true;
        }

        /// <summary>
        /// Allows customizing where the slime will be positioned during a hug
        /// </summary>
        public virtual Vector2 GetHugOffset(CuteSlimeBaseProj slime, PetPlayer petPlayer)
        {
            return default;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int CompareTo(SlimeHug other)
        {
            return Cooldown.CompareTo(other.Cooldown);
        }
    }
}
