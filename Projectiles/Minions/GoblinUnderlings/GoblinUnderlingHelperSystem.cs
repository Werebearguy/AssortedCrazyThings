using AssortedCrazyThings.Base;
using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingHelperSystem : AssSystem
	{
		public static IEnumerable<Projectile> GetLocalGoblinUnderlings(GoblinUnderlingChatterType guChatterType = GoblinUnderlingChatterType.None)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];

				if (!proj.active || proj.owner != Main.myPlayer)
				{
					continue;
				}

				if (guChatterType == GoblinUnderlingChatterType.None && GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(proj.type))
				{
					yield return proj;
				}
				else if (GoblinUnderlingTierSystem.GoblinUnderlingProjs.TryGetValue(proj.type, out var value) && value == guChatterType)
				{
					yield return proj;
				}
			}
		}

		//TODO goblin Move to set registration + GlobalProjectile ? doesnt work for everything though (see terra beam)
		/// <summary>
		/// Handles kb scaling and knocking away from player. Should be called from all projectiles
		/// </summary>
		public static void CommonModifyHitNPC(GoblinUnderlingClass currentClass, Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			var tier = GoblinUnderlingTierSystem.GetCurrentTierStats(currentClass);
			//damage = (int)(damage * tier.damageMult); THIS IS DONE IN GOBLIN PREAI, OVERRIDING DEFAULT MINION SCALING

			modifiers.Knockback *= tier.knockbackMult;

			modifiers.ArmorPenetration += tier.armorPen;

			float fromPlayerToTargetX = target.Center.X - projectile.GetOwner().Center.X;
			if (Math.Abs(fromPlayerToTargetX) < 7 * 16)
			{
				//Hit away from player if target is close
				modifiers.HitDirectionOverride = Math.Sign(fromPlayerToTargetX);
				modifiers.Knockback *= 2;
			}
		}
	}
}
