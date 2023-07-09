namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	/// <summary>
	/// Should be used for anything that has no conditions. The default if not specified for a <see cref="ChatterMessage"/>. Will be the lowest priority for <see cref="ChatterMessageGroup.PoolsByPriority"/>
	/// </summary>
	public class AlwaysTrueChatterCondition : ChatterCondition
	{
		protected override bool Check(ChatterSource source, IChatterParams param)
		{
			return true;
		}
	}
}
