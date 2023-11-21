using System;

namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	public class GoblinUnderlingDialogueChatterMessage : ChatterMessage
	{
		public GoblinUnderlingChatterType GUChatterType { get; init; }
		public Action<bool, DialogueChatterParams> OnMessage { get; init; }

		//No conditions, to make sure only one pool is used
		public GoblinUnderlingDialogueChatterMessage(GoblinUnderlingChatterType guChatterType, string key, Action<bool, DialogueChatterParams> onMessage = null) : base(key + guChatterType.ToString())
		{
			GUChatterType = guChatterType;
			OnMessage = onMessage;
		}
	}
}
