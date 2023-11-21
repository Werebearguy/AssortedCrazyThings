using AssortedCrazyThings.Base.Data;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	//Helpers
	public abstract partial class ChatterCondition
	{
		public static bool MatchesNPCType(IChatterParams param, int type)
		{
			if (param is BossDefeatChatterParams p)
			{
				if (p.Type == type)
				{
					return true;
				}
			}
			else if (param is BossSpawnChatterParams p2)
			{
				if (p2.NPC.type == type)
				{
					return true;
				}
			}

			return false;
		}

		public static bool JustChangedInvasion(IChatterParams param, int invasionTypeToCheck)
		{
			if (param is not InvasionChangedChatterParams p)
			{
				return false;
			}

			var current = p.Current;
			var prev = p.Prev;
			if (current == prev || current != invasionTypeToCheck)
			{
				//Not the invasion we want/Nothing changed
				return false;
			}

			return true;
		}

		public static bool JustSelectedItem(IChatterParams param, int itemTypeToCheck)
		{
			if (param is not ItemSelectedChatterParams p)
			{
				return false;
			}

			var current = p.Current;
			var prev = p.Prev;
			if (current != itemTypeToCheck || current == prev)
			{
				//Not the item we want/Nothing changed
				return false;
			}

			return true;
		}

		public static bool JustEquipped(IChatterParams param, EquipSnapshot equipToCheck)
		{
			if (param is not ArmorEquipChatterParams p)
			{
				return false;
			}

			//JustEquipped behavior means: if prev is not equal to current (change happened), and current is what we want

			//equipToCheck is a mask: -1 means we don't care what that value is
			//example:
			//3 4 -1
			//This means last element in current/prev will get ignored
			var current = p.CurrentEquips.GetMasked(equipToCheck);
			var prev = p.PrevEquips.GetMasked(equipToCheck);
			//Now use those that only contain the stat we care about
			if (current.Equals(prev))
			{
				//Nothing changed
				return false;
			}

			return current.Equals(equipToCheck);
		}
	}
}
