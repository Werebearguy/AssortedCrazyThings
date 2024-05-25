using System;
using System.Collections.Generic;
using Terraria;

namespace AssortedCrazyThings.Base.Handlers.ProgressionTierHandler
{
	/// <summary>
	/// Tracker for progression tiers. Tracks a global tier, and then the specified tier set, matching the highest available
	/// </summary>
	public abstract class ProgressionTierSystem : AssSystem
	{
		public ProgressionTierSet TierSet { get; private set; }
		public ProgressionTierStage CurrentTier { get; private set; }
		public int CurrentTierIndex => (int)CurrentTier;

		private static bool updatedGlobalTier = false;
		private static ProgressionTierStage CurrentGlobalTier { get; set; }
		private static int CurrentGlobalTierIndex => (int)CurrentGlobalTier;

		private static Dictionary<ProgressionTierStage, Func<bool>> globalTiers;
		public static int GlobalTierCount => globalTiers.Count;

		protected abstract ProgressionTierSet GetTierSet();

		public sealed override void OnModLoad()
		{
			if (globalTiers == null)
			{
				globalTiers = new Dictionary<ProgressionTierStage, Func<bool>>()
				{
					{ ProgressionTierStage.PreBoss, () => true },
					{ ProgressionTierStage.EoC, () => NPC.downedBoss1 },
					{ ProgressionTierStage.Evil, () => NPC.downedBoss2 },
					{ ProgressionTierStage.Skeletron, () => NPC.downedBoss3 },
					{ ProgressionTierStage.WoF, () => Main.hardMode },
					{ ProgressionTierStage.Mech, () => NPC.downedMechBossAny },
					{ ProgressionTierStage.Plantera, () => NPC.downedPlantBoss },
					{ ProgressionTierStage.Cultist, () => NPC.downedAncientCultist },
				};
			}

			SafeOnModLoad();

			TierSet = GetTierSet();
		}

		public virtual void SafeOnModLoad()
		{

		}

		private void DetermineCurrentGlobalTier()
		{
			CurrentGlobalTier = ProgressionTierStage.PreBoss;
			for (int i = GlobalTierCount - 1; i >= 0; i--)
			{
				var tier = (ProgressionTierStage)i;
				//Start from last tier, prioritize
				var tierCondition = globalTiers[tier];
				if (tierCondition.Invoke())
				{
					CurrentGlobalTier = tier;
					break;
				}
			}
		}

		private void DetermineCurrentTier()
		{
			CurrentTier = ProgressionTierStage.PreBoss;
			int global = CurrentGlobalTierIndex;
			//Get highest compatible tier
			var tiers = TierSet.Tiers;
			for (int i = tiers.Length - 1; i >= 0; i--)
			{
				var tier = tiers[i];
				if ((int)tiers[i] <= global)
				{
					CurrentTier = tier;
					break;
				}
			}
		}

		public sealed override void PostUpdatePlayers()
		{
			if (!updatedGlobalTier)
			{
				//Do it only once
				DetermineCurrentGlobalTier();
				updatedGlobalTier = true;
			}
			DetermineCurrentTier();
		}

		public sealed override void PostUpdateEverything()
		{
			updatedGlobalTier = false;
		}
	}
}
