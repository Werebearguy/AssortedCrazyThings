namespace AssortedCrazyThings.Base.Chatter
{
	public class BossDefeatChatterParams : IChatterParams
	{
		public int Type { get; init; }

		public BossDefeatChatterParams(int type)
		{
			Type = type;
		}
	}
}
