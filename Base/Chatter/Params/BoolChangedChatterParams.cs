namespace AssortedCrazyThings.Base.Chatter
{
	public class BoolChangedChatterParams : IChatterParams
	{
		public bool Current { get; init; }
		public bool Prev { get; init; }

		public BoolChangedChatterParams(bool current, bool prev)
		{
			Current = current;
			Prev = prev;
		}
	}
}
