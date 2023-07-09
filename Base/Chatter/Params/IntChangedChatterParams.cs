namespace AssortedCrazyThings.Base.Chatter
{
	public class IntChangedChatterParams : IChatterParams
	{
		public int Current { get; init; }
		public int Prev { get; init; }

		public IntChangedChatterParams(int current, int prev)
		{
			Current = current;
			Prev = prev;
		}
	}
}
