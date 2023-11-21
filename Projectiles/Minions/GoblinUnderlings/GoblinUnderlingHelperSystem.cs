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
		public override void OnModLoad()
		{
			On_Player.FreeUpPetsAndMinions += On_Player_FreeUpPetsAndMinions;
		}

		private static void On_Player_FreeUpPetsAndMinions(On_Player.orig_FreeUpPetsAndMinions orig, Player self, Item sItem)
		{
			bool atleastOne = false;
			if (self.altFunctionUse == 2)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile p = Main.projectile[i];
					if (p.active && p.owner == self.whoAmI && GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(p.type))
					{
						atleastOne = true;
						p.minion = false; // temporarily de-minion it so that it doesn't get sacrificed
					}
				}
			}

			orig(self, sItem);

			// re-minionify all necessary minions
			if (atleastOne)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile p = Main.projectile[i];
					if (p.active && p.owner == self.whoAmI && !p.minion && GoblinUnderlingTierSystem.GoblinUnderlingProjs.ContainsKey(p.type))
					{
						p.minion = true;
					}
				}
			}
		}

		public static Projectile GetFirstGoblinUnderling(GoblinUnderlingChatterType guChatterType)
		{
			foreach (var proj in GetLocalGoblinUnderlings(guChatterType))
			{
				return proj;
			}

			return null;
		}

		public static IEnumerable<Projectile> GetLocalGoblinUnderlings(GoblinUnderlingChatterType guChatterType = GoblinUnderlingChatterType.All)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];

				if (!proj.active || proj.owner != Main.myPlayer)
				{
					continue;
				}
				
				if (GoblinUnderlingTierSystem.GoblinUnderlingProjs.TryGetValue(proj.type, out var value) && guChatterType.HasFlag(value))
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
