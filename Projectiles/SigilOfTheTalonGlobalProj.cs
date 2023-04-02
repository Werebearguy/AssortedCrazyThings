using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Accessories.Useful;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles
{
	[Content(ContentType.Bosses)]
	public class SigilOfTheTalonGlobalProj : AssGlobalProjectile
	{
		public bool[] HitNPCs { get; private set; } = new bool[Main.maxNPCs];

		public static readonly int MaxPenetrateCount = 3;
		public int PenetrateCount { get; private set; } = MaxPenetrateCount;

		public int GetDamageDropoff(int damage)
		{
			float ratio = (float)PenetrateCount / MaxPenetrateCount;
			return Math.Max(1, (int)(damage * Utils.Remap(ratio, 0f, 1f, SigilOfTheTalon.FirstDamageDropOff / 100f / MaxPenetrateCount, SigilOfTheTalon.FirstDamageDropOff / 100f))); //To avoid reports of "it doesnt work", min 1 damage
		}

		public override bool InstancePerEntity => true;

		public static bool Extending(Projectile projectile) => projectile.ai[0] == 0;

		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
		{
			return lateInstantiation && entity.aiStyle == ProjAIStyleID.Hook;
		}

		public override GlobalProjectile Clone(Projectile from, Projectile to)
		{
			var clone = (SigilOfTheTalonGlobalProj)base.Clone(from, to);

			clone.HitNPCs = new bool[HitNPCs.Length];
			Array.Copy(HitNPCs, clone.HitNPCs, clone.HitNPCs.Length);

			return clone;
		}

		public override void PostAI(Projectile projectile)
		{
			if (!Extending(projectile))
			{
				return;
			}

			if (!projectile.GetOwner().GetModPlayer<AssPlayer>().sigilOfTheTalon)
			{
				return;
			}

			Visuals(projectile);

			DamageHostiles(projectile);
		}

		public static void Visuals(Projectile projectile)
		{
			for (int i = 0; i < 2; i++)
			{
				if (true || Main.rand.NextBool())
				{
					Dust dust = Dust.NewDustPerfect(projectile.Center, 135, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), 0, Color.White * 0.8f, 0.8f);
					dust.noGravity = true;
					dust.noLight = true;
					dust.fadeIn = Main.rand.NextFloat(0.6f, 0.9f);
				}
			}
		}

		private void DamageHostiles(Projectile projectile)
		{
			if (Main.myPlayer != projectile.owner)
			{
				return;
			}

			if (PenetrateCount <= 0)
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
						player.ApplyDamageToNPC(npc, damage, 0f, 0, crit: false);
						damage = GetDamageDropoff(originalDamage); //Recalculate immediately in case this hits multiple enemies in one tick
					}
				}
			}
		}
	}
}
