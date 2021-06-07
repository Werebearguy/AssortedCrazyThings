using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace AssortedCrazyThings.NPCs.DropRules
{
    class OneFromOptionsPerPlayerOnPlayerRule : IItemDropRule
	{
		public int[] dropIds;
		public int chanceDenominator;
		public int chanceNumerator;
		public int amountDroppedMinimum;
		public int amountDroppedMaximum;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public OneFromOptionsPerPlayerOnPlayerRule(int chanceDenominator = 1, int chanceNumerator = 1, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, params int[] options)
		{
			this.chanceDenominator = chanceDenominator;
			this.chanceNumerator = chanceNumerator;
			this.amountDroppedMinimum = amountDroppedMinimum;
			this.amountDroppedMaximum = amountDroppedMaximum;
			dropIds = options;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info) => true;

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			CommonCode.DropItemForEachInteractingPlayerOnThePlayer(info.npc, info.rng.Next(dropIds), info.rng, chanceNumerator, chanceDenominator, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
			ItemDropAttemptResult result = new ItemDropAttemptResult()
			{
				State = ItemDropAttemptResultState.Success
			};
			return result;
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = chanceNumerator / (float)chanceDenominator;
			float num2 = num * ratesInfo.parentDroprateChance;
			float dropRate = 1f / dropIds.Length * num2;
			for (int i = 0; i < dropIds.Length; i++)
			{
				drops.Add(new DropRateInfo(dropIds[i], 1, 1, dropRate, ratesInfo.conditions));
			}

			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}
	}
}
