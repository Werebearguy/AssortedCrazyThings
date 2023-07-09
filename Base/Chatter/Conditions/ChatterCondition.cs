using System;

namespace AssortedCrazyThings.Base.Chatter.Conditions
{
	/// <summary>
	/// Evaluator for <see cref="IChatterParams">. Only accepts a specific <see cref="IChatterParams"> instance (or <see cref="NoChatterParams"/>), register in <see cref="ChatterSystem.SourceToParamTypes"/>
	/// </summary>
	public abstract partial class ChatterCondition
	{
		public bool IsTrue(ChatterSource source, IChatterParams param)
		{
			if (param is NoChatterParams)
			{
				return true;
			}

			Type ourType = param.GetType();
			Type expectedType = ChatterSystem.SourceToParamTypes[source];
			if (ourType != expectedType)
			{
				throw new Exception($"Expected param type {expectedType} does not match provided type {ourType} for {source}!");
			}

			return Check(source, param);
		}

		protected abstract bool Check(ChatterSource source, IChatterParams param);

		public override string ToString()
		{
			return this.GetType().Name;
		}
	}
}
