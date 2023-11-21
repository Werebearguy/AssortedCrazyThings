using AssortedCrazyThings.Base.Chatter.Conditions;
using Terraria.Localization;

namespace AssortedCrazyThings.Base.Chatter
{
	/// <summary>
	/// The message, mainly a container for the (localized) text, and a condition
	/// </summary>
	public class ChatterMessage
	{
		/// <summary>
		/// Unique key ("internal name")
		/// </summary>
		public string Key { get; init; }

		//Assigned in PostSetupContent
		public LocalizedText Text { get; set; }

		/// <summary>
		/// Defaults to <see cref="AlwaysTrueChatterCondition"/>
		/// </summary>
		public ChatterCondition Condition { get; init; }

		/// <summary>
		/// If true, and if <see cref="Condition"/> will be fullfilled, will be added to the pool of messages which have no condition (<see cref="AlwaysTrueChatterCondition"/>) instead of being checked first/separately
		/// </summary>
		public bool AddedToAlwaysTruePool { get; init; }

		// Lang registration takes place after initialization, to map to type and source
		public ChatterMessage(string key, ChatterCondition condition = null, bool addedToNoConditionPool = false)
		{
			Key = key;
			Text = LocalizedText.Empty;
			Condition = condition ?? new AlwaysTrueChatterCondition();
			AddedToAlwaysTruePool = addedToNoConditionPool;
		}

		public override string ToString()
		{
			string text = Text.ToString();
			return text == string.Empty ? Key : text;
		}
	}
}
