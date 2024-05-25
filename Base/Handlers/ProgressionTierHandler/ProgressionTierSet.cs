using System;
using System.Linq;

namespace AssortedCrazyThings.Base.Handlers.ProgressionTierHandler
{
	public class ProgressionTierSet
	{
		private readonly ProgressionTierStage[] tiers;
		public ReadOnlySpan<ProgressionTierStage> Tiers => tiers;
		public int Count => tiers.Length;

		public ProgressionTierSet(ProgressionTierStage[] tiers)
		{
			if (!tiers.Contains(ProgressionTierStage.PreBoss))
			{
				throw new Exception($"{nameof(tiers)} must contain {ProgressionTierStage.PreBoss}!");
			}

			this.tiers = new ProgressionTierStage[tiers.Length];
			Array.Copy(tiers, this.tiers, tiers.Length);

			//Make sure they are ordered
			this.tiers = this.tiers.OrderBy(stage => (int)stage).ToArray();
		}
	}
}
