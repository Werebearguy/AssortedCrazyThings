using AssortedCrazyThings.Base.Chatter.Conditions;

namespace AssortedCrazyThings.Base.Chatter
{
	/// <summary>
	/// Special case that will be evaluated to always true by ANY <see cref="ChatterCondition"/>
	/// </summary>
	public class NoChatterParams : IChatterParams
	{
		public NoChatterParams()
		{

		}
	}
}
