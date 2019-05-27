using Microsoft.Xna.Framework;
using AssortedCrazyThings.Base;
using System;
using Terraria;
using Terraria.ModLoader;
using AssortedCrazyThings.Items.Weapons;
using Terraria.ID;
using System.IO;

namespace AssortedCrazyThings.Projectiles.Minions
{
    /// <summary>
    /// Uses ai[1] for the minion position and localAI[0] & localAI[1] for the bobbing and a random number.
    /// Bobbing (sinY) needs to be implemented manually in some draw hook
    /// </summary>
    public abstract class DroneBase : ModProjectile
    {
        /// <summary>
        /// Custom MinionPos to determine position
        /// </summary>
        protected int MinionPos
        {
            get
            {
                return (int)projectile.ai[1];
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
        /// Need to sync manually in the inheriting class
        /// </summary>
        protected byte RandomNumber
        {
            get
            {
                return (byte)projectile.localAI[1];
            }
            set
            {
                projectile.localAI[1] = value;
            }
        }

        protected bool RealOwner
        {
            get
            {
                return Main.netMode != NetmodeID.Server && projectile.owner == Main.myPlayer;
            }
        }

        protected virtual bool IsCombatDrone
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Depends on projectile.localAI[0]
        /// </summary>
        protected float sinY;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)RandomNumber);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            RandomNumber = reader.ReadByte();
        }

        protected virtual void CustomAI()
        {

        }

        protected virtual void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
        {

        }

        protected virtual void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            AssPlayer modPlayer = player.GetModPlayer<AssPlayer>(mod);
            if (player.dead)
            {
                modPlayer.droneControllerMinion = false;
            }
            if (modPlayer.droneControllerMinion)
            {
                projectile.timeLeft = 2;
            }
        }

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
            Player player = Main.player[projectile.owner];
            Vector2 offset = new Vector2(-30, 20); //to offset FlickerwickPetAI to player.Center
            offset += DroneController.GetPosition(projectile, MinionPos);
            if (RandomNumber == 0)
            {
                RandomNumber = (byte)Main.rand.Next(1, 256);
            }
            if (!IsCombatDrone)
            {
                MinionPos = 0;
            }

            bool staticDirection = true;
            bool reverseSide = false;
            float veloXToRotationFactor = 0.5f + (RandomNumber / 255f - 0.5f) * 0.5f;
            float veloSpeed = 1f + (RandomNumber / 255f - 0.5f) * 0.4f;
            float offsetX = offset.X;
            float offsetY = offset.Y;
            bool run = ModifyDefaultAI(ref staticDirection, ref reverseSide, ref veloXToRotationFactor, ref veloSpeed, ref offsetX, ref offsetY);
            if (run)
            {
                AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, staticDirection: staticDirection, reverseSide: reverseSide, veloXToRotationFactor: veloXToRotationFactor, veloSpeed: veloSpeed, offsetX: offsetX, offsetY: offsetY);
                projectile.direction = projectile.spriteDirection = -player.direction;
            }
            player.numMinions--; //make it so it doesn't affect projectile.minionPos of non-drone minions

            #endregion

            CustomAI();
            Bobbing();
            CustomFrame();
        }
    }
}
