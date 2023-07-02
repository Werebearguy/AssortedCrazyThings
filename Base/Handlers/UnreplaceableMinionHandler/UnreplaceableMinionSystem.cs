using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Base.Handlers.UnreplaceableMinionHandler
{
	//Taken with permission from billybobmcbo
	[Content(ConfigurationSystem.AllFlags)]
	public class UnreplaceableMinionSystem : AssSystem
	{
		private static HashSet<int> Minions;

		public static bool Add(int type)
		{
			return Minions.Add(type);
		}

		public static bool Exists(int type)
		{
			return Minions.Contains(type);
		}

		public override void OnModLoad()
		{
			Minions = new HashSet<int>();

			On_Player.FreeUpPetsAndMinions += On_Player_FreeUpPetsAndMinions;
		}

		public override void Unload()
		{
			Minions = null;
		}

		private static void On_Player_FreeUpPetsAndMinions(On_Player.orig_FreeUpPetsAndMinions orig, Player self, Item sItem)
		{
			bool atleastOne = false;
			if (ProjectileID.Sets.MinionSacrificable[sItem.shoot])
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile p = Main.projectile[i];
					if (p.active && p.owner == self.whoAmI && Exists(p.type))
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
					if (p.active && p.owner == self.whoAmI && !p.minion && Exists(p.type))
					{
						p.minion = true;
					}
				}
			}
		}
	}
}
