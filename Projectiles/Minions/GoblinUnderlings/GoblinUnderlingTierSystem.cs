using AssortedCrazyThings.Base.Chatter.GoblinUnderlings;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings
{
	public enum GoblinUnderlingProgressionTierStage : int
	{
		//Value important, texture index
		PreBoss = 0,
		EoC = 1,
		Evil = 2,
		Skeletron = 3,
		Mech = 4,
		Plantera = 5,
		Cultist = 6
	}

	[Content(ContentType.Weapons)]
	public class GoblinUnderlingTierSystem : AssSystem
	{
		public static Dictionary<int, GoblinUnderlingChatterType> GoblinUnderlingProjs { get; private set; }

		private static Dictionary<int, Dictionary<GoblinUnderlingProgressionTierStage, GoblinUnderlingTierStats>> tierStats = new();
		private static Dictionary<GoblinUnderlingProgressionTierStage, Func<bool>> tiers = new();

		public static int TierCount => tiers.Count;
		public static GoblinUnderlingProgressionTierStage CurrentTier { get; private set; }
		public static int CurrentTierIndex => (int)CurrentTier;

		public static List<GoblinUnderlingProgressionTierStage> GetTiers()
		{
			return tiers.Keys.ToList();
		}

		public static void RegisterStats(int type, Dictionary<GoblinUnderlingProgressionTierStage, GoblinUnderlingTierStats> stats)
		{
			tierStats[type] = stats;
		}

		public static GoblinUnderlingTierStats GetCurrentTierStats(int type)
		{
			return tierStats[type][CurrentTier];
		}

		private static void LoadTiers()
		{
			tiers = new Dictionary<GoblinUnderlingProgressionTierStage, Func<bool>>()
			{
				{ GoblinUnderlingProgressionTierStage.PreBoss, () => true },
				{ GoblinUnderlingProgressionTierStage.EoC, () => NPC.downedBoss1 },
				{ GoblinUnderlingProgressionTierStage.Evil, () => NPC.downedBoss2 },
				{ GoblinUnderlingProgressionTierStage.Skeletron, () => NPC.downedBoss3 },
				{ GoblinUnderlingProgressionTierStage.Mech, () => NPC.downedMechBossAny },
				{ GoblinUnderlingProgressionTierStage.Plantera, () => NPC.downedPlantBoss },
				{ GoblinUnderlingProgressionTierStage.Cultist, () => NPC.downedAncientCultist },
			};
		}

		private static void DetermineCurrentProgressionTier()
		{
			CurrentTier = GoblinUnderlingProgressionTierStage.PreBoss;
			for (int i = TierCount - 1; i >= 0; i--)
			{
				var tier = (GoblinUnderlingProgressionTierStage)i;
				//Start from last tier, prioritize
				var tierCondition = tiers[tier];
				if (tierCondition.Invoke())
				{
					CurrentTier = tier;
					break;
				}
			}
		}

		public override void OnModLoad()
		{
			GoblinUnderlingProjs = new();

			LoadTiers();
		}

		public override void Unload()
		{
			GoblinUnderlingProjs = null;

			tiers = null;
			tierStats = null;
		}

		public override void PostUpdatePlayers()
		{
			DetermineCurrentProgressionTier();
		}
	}
}
