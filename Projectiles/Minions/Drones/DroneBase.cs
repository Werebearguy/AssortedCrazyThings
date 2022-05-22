using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Minions.Drones
{
	/// <summary>
	/// Uses ai[0] for a counter and ai[1] for the minion position.
	/// LocalAI[0] & localAI[1] for the bobbing and a random number.
	/// Bobbing (sinY) needs to be implemented manually in some draw hook
	/// </summary>
	[Content(ContentType.Weapons)]
	public abstract class DroneBase : AssProjectile
	{

		/// <summary>
		/// Currently only used to make MinionPos 0 again. The assignment of MinionPos still depends on the array used in Shoot()
		/// </summary>
		public virtual bool IsCombatDrone
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Custom MinionPos to determine position
		/// </summary>
		public int MinionPos
		{
			get
			{
				return (int)Projectile.ai[1];
			}
			set
			{
				Projectile.ai[1] = value;
			}
		}

		/// <summary>
		/// General purpose counter using ai[0]
		/// </summary>
		protected int Counter
		{
			get
			{
				return (int)Projectile.ai[0];
			}
			set
			{
				Projectile.ai[0] = value;
			}
		}

		protected float Sincounter
		{
			get
			{
				return Projectile.localAI[0];
			}
			set
			{
				Projectile.localAI[0] = value;
			}
		}

		protected byte RandomNumber
		{
			get
			{
				return (byte)Projectile.localAI[1];
			}
			set
			{
				Projectile.localAI[1] = value;
			}
		}

		/// <summary>
		/// Combined != Server and owner == myPlayer check
		/// </summary>
		protected bool RealOwner
		{
			get
			{
				return Main.netMode != NetmodeID.Server && Projectile.owner == Main.myPlayer;
			}
		}

		/// <summary>
		/// Use this when spawning projectiles
		/// </summary>
		protected int CustomDmg
		{
			get
			{
				return (int)(Projectile.damage * dmgModifier);
			}
		}

		/// <summary>
		/// Use this when spawning projectiles
		/// </summary>
		protected float CustomKB
		{
			get
			{
				return Projectile.knockBack * kbModifier;
			}
		}

		/// <summary>
		/// Depends on projectile.localAI[0]
		/// </summary>
		protected float sinY = 0f;
		private float dmgModifier = 1f;
		private float kbModifier = 1f;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)RandomNumber);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			RandomNumber = reader.ReadByte();
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (IsCombatDrone)
			{
				MinionPos = DroneController.GetSlotOfNextDrone(Projectile);
			}
		}

		protected virtual void CustomAI()
		{

		}

		protected virtual void CustomFrame(int frameCounterMaxFar = 4, int frameCounterMaxClose = 8)
		{

		}

		protected virtual void CheckActive()
		{
			Player player = Projectile.GetOwner();
			AssPlayer modPlayer = player.GetModPlayer<AssPlayer>();
			if (player.dead)
			{
				modPlayer.droneControllerMinion = false;
			}
			if (modPlayer.droneControllerMinion)
			{
				Projectile.timeLeft = 2;
			}
		}

		/// <summary>
		/// Bobbing logic. Implement sinY yourself. Return false to not let default AI calculate it
		/// </summary>
		protected virtual bool Bobbing()
		{
			return true;
		}

		/// <summary>
		/// Use to decide what happens when the player holds the Drone Controller.
		/// Can be used to change damage and knockback, or internal timers
		/// </summary>
		protected virtual void ModifyDroneControllerHeld(ref float dmgModifier, ref float kbModifier)
		{

		}

		/// <summary>
		/// Use to change any default values of the AI on the fly (called before Default AI is called).
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
			if (RandomNumber == 0)
			{
				RandomNumber = (byte)Main.rand.Next(1, 256);
			}

			Player player = Projectile.GetOwner();
			Vector2 offset = new Vector2(-30, 20); //to offset FlickerwickPetAI to player.Center
			offset += DroneController.GetPosition(Projectile, MinionPos);

			bool staticDirection = true;
			bool reverseSide = false;
			float veloXToRotationFactor = 0.5f + (RandomNumber / 255f - 0.5f) * 0.5f;
			float veloSpeed = 1f + (RandomNumber / 255f - 0.5f) * 0.4f;
			float offsetX = offset.X;
			float offsetY = offset.Y;
			bool run = ModifyDefaultAI(ref staticDirection, ref reverseSide, ref veloXToRotationFactor, ref veloSpeed, ref offsetX, ref offsetY);
			if (run)
			{
				AssAI.FlickerwickPetAI(Projectile, lightPet: false, lightDust: false, staticDirection: staticDirection, reverseSide: reverseSide, veloXToRotationFactor: veloXToRotationFactor, veloSpeed: veloSpeed, offsetX: offsetX, offsetY: offsetY);
				Projectile.direction = Projectile.spriteDirection = -player.direction;
			}
			if (IsCombatDrone) player.numMinions--; //make it so it doesn't affect projectile.minionPos of non-drone minions

			#endregion

			dmgModifier = 1f;
			kbModifier = 1f;
			if (player.HeldItem.type == ModContent.ItemType<DroneController>())
			{
				ModifyDroneControllerHeld(ref dmgModifier, ref kbModifier);
			}
			CustomAI();
			if (Bobbing())
			{
				Sincounter = Sincounter > 240 ? 0 : Sincounter + 1;
				sinY = (float)((Math.Sin(((Sincounter + MinionPos * 10f) / 120f) * MathHelper.TwoPi) - 1) * 4);
			}
			CustomFrame();
		}
	}
}
