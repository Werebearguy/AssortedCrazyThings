using Microsoft.Xna.Framework;
using AssortedCrazyThings.Base;
using System;
using Terraria;
using Terraria.ModLoader;
using AssortedCrazyThings.Items.Weapons;

namespace AssortedCrazyThings.Projectiles.Minions
{
    /// <summary>
    /// Uses only ai[1] for the minion position and localAI[0] for the bobbing.
    /// Bobbing needs to be implemented manually in some draw hook
    /// </summary>
    public abstract class CombatDroneBase : ModProjectile
    {
        /// <summary>
        /// Custom MinionPos to determine position
        /// </summary>
        protected float MinionPos
        {
            get
            {
                return projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }

        protected float Sincounter
        {
            get
            {
                return projectile.localAI[0];
            }
            set
            {
                projectile.localAI[0] = value;
            }
        }

        /// <summary>
        /// depends on projectile.localAI[0]
        /// </summary>
        protected float sinY;

        protected virtual void CustomAI()
        {

        }

        protected virtual void CustomDraw(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {

        }

        protected abstract void CheckActive();

        protected virtual void Bobbing()
        {
            Sincounter = Sincounter > 240 ? 0 : Sincounter + 1;
            sinY = (float)((Math.Sin(((Sincounter + MinionPos * 10f) / 120f) * 2 * Math.PI) - 1) * 4);
        }

        /// <summary>
        /// To to change any default values of the AI on the fly (called before Default AI is called).
        /// Return false to prevent the default AI to run
        /// </summary>
        protected virtual bool ModifyDefaultAI(ref bool staticDirection, ref bool reverseSide, ref float veloXToRotationFactor, ref float veloSpeed, ref float offsetX, ref float offsetY)
        {
            return true;
        }

        public sealed override void AI()
        {
            CheckActive();

            #region Default AI
            Vector2 offset = new Vector2(-30, 20); //to offset FlickerwickPetAI to player.Center
            offset += DroneController.GetPosition(projectile, (int)MinionPos);
            bool staticDirection = true;
            bool reverseSide = false;
            float veloXToRotationFactor = 0.5f;
            float veloSpeed = 1f;
            float offsetX = offset.X;
            float offsetY = offset.Y;
            bool run = ModifyDefaultAI(ref staticDirection, ref reverseSide, ref veloXToRotationFactor, ref veloSpeed, ref offsetX, ref offsetY);
            if (run)
            {
                AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, staticDirection: staticDirection, reverseSide: reverseSide, veloXToRotationFactor: veloXToRotationFactor, veloSpeed: veloSpeed, offsetX: offsetX, offsetY: offsetY);
                projectile.direction = projectile.spriteDirection = -Main.player[projectile.owner].direction;
            }
            #endregion

            CustomAI();
            Bobbing();
            CustomDraw();
        }
    }
}
