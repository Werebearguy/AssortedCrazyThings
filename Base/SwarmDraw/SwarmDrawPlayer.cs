using AssortedCrazyThings.Base.SwarmDraw.FairySwarmDraw;
using AssortedCrazyThings.Base.SwarmDraw.SwarmofCthulhuDraw;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base.SwarmDraw
{
	//TODO unhardcode this when both a better tml loader comes along and more swarm draw sets exist
	[Content(ConfigurationSystem.AllFlags)]
	public class SwarmDrawPlayer : AssPlayerBase
	{
		public static void HandleDrawSet(ref SwarmDrawSet set, Func<SwarmDrawSet> gen, bool condition, Vector2 center)
		{
			if (condition)
			{
				if (set == null)
				{
					set = gen();
				}

				if (set == null)
				{
					return;
				}

				if (!set.Active)
				{
					set.Activate();
				}

				set.Update(center);
			}
			else if (set != null)
			{
				set.Deactivate();
			}
		}

		public SwarmDrawSet fairySwarmDrawSet;

		public SwarmDrawSet swarmofCthulhuDrawSet;

		public override void Initialize()
		{
			fairySwarmDrawSet = null;
			swarmofCthulhuDrawSet = null;
		}

		public override void PostUpdate()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (ContentConfig.Instance.DroppedPets)
			{
				HandleDrawSet(ref fairySwarmDrawSet,
					SwarmDrawSet.New<FairySwarmDrawSet>,
					Player.ownedProjectileCounts[ModContent.ProjectileType<FairySwarmProj>()] > 0,
					Player.Center);

				HandleDrawSet(ref swarmofCthulhuDrawSet,
					SwarmDrawSet.New<SwarmofCthulhuDrawSet>,
					Player.ownedProjectileCounts[ModContent.ProjectileType<SwarmofCthulhuProj>()] > 0,
					Player.Center);
			}
		}
	}
}
