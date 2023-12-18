using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Accessories.Useful;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Projectiles.Accessories
{
	[Content(ContentType.Bosses)]
	public class SigilOfTheTalonGlobalProj : AssGlobalProjectile
	{
		public bool[] HitNPCs { get; private set; } = new bool[Main.maxNPCs];

		public static byte MaxPenetrateCount => (byte)SigilOfTheTalon.MaxPierce;
		public byte PenetrateCount { get; private set; } = MaxPenetrateCount;

		public bool PrevLatched { get; private set; } = false;

		public bool AtleastOneHit => PenetrateCount < MaxPenetrateCount;

		public override bool InstancePerEntity => true;

		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return lateInstantiation && entity.aiStyle == ProjAIStyleID.Hook;
		}

		public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
		{
			//Code based on MaxPenetrateCount being only 0 1 2 3 -> 2 bits
			bitWriter.WriteBit((PenetrateCount & 1) == 1);
			bitWriter.WriteBit((PenetrateCount & 2) == 2);
		}

		public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
		{
			var bits = new BitsByte();
			bits[0] = bitReader.ReadBit();
			bits[1] = bitReader.ReadBit();
			PenetrateCount = bits;
		}

		public override GlobalProjectile Clone(Projectile from, Projectile to)
		{
			var clone = (SigilOfTheTalonGlobalProj)base.Clone(from, to);

			clone.HitNPCs = new bool[HitNPCs.Length];
			Array.Copy(HitNPCs, clone.HitNPCs, clone.HitNPCs.Length);

			return clone;
		}

		public override bool PreAI(Projectile projectile)
		{
			PrevLatched = Latched(projectile);

			return base.PreAI(projectile);
		}

		public override void PostAI(Projectile projectile)
		{
			if (!projectile.GetOwner().GetModPlayer<AssPlayer>().sigilOfTheTalon)
			{
				return;
			}

			if (Latched(projectile) && !PrevLatched)
			{
				//Between Pre and Post Latch status was achieved
				if (AtleastOneHit)
				{
					projectile.ai[0] = 1f; //Retract
					projectile.netUpdate = true;
				}
			}

			if (!Extending(projectile))
			{
				return;
			}

			Visuals(projectile);

			DamageHostiles(projectile);
		}

		public static bool Extending(Projectile projectile) => projectile.ai[0] == 0;
		public static bool Latched(Projectile projectile) => projectile.ai[0] == 2;

		public static void Visuals(Projectile projectile)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, 135, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), 0, Color.White * 0.8f, 0.8f);
				dust.noGravity = true;
				dust.noLight = true;
				dust.fadeIn = Main.rand.NextFloat(0.6f, 0.9f);
			}
		}

		public int GetDamageDropoff(int damage)
		{
			float ratio = (float)PenetrateCount / MaxPenetrateCount;
			return Math.Max(1, (int)(damage * Utils.Remap(ratio, 0f, 1f, SigilOfTheTalon.FirstDamageDropOff / 100f / MaxPenetrateCount, SigilOfTheTalon.FirstDamageDropOff / 100f))); //To avoid reports of "it doesnt work", min 1 damage
		}

		private void DamageHostiles(Projectile projectile)
		{
			if (Main.myPlayer != projectile.owner)
			{
				return;
			}

			if (PenetrateCount == 0)
			{
				return;
			}

			Player player = projectile.GetOwner();
			var mPlayer = player.GetModPlayer<AssPlayer>();
			int originalDamage = mPlayer.LastSelectedWeaponDamage;
			int damage = GetDamageDropoff(originalDamage);
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && !npc.friendly && (npc.damage > 0 || npc.type == NPCID.TargetDummy) && !npc.dontTakeDamage && player.CanNPCBeHitByPlayerOrPlayerProjectile(npc))
				{
					if (!HitNPCs[i] && projectile.Hitbox.Intersects(npc.Hitbox))
					{
						HitNPCs[i] = true;
						PenetrateCount--;
						projectile.netUpdate = true;
						player.ApplyDamageToNPC(npc, damage, 0f, 0, crit: false);
						damage = GetDamageDropoff(originalDamage); //Recalculate immediately in case this hits multiple enemies in one tick
					}
				}
			}
		}
	}
}
