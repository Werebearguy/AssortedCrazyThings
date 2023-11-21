namespace AssortedCrazyThings.Base.Chatter.GoblinUnderlings
{
	public class GoblinUnderlingDialogueParticipantOrder
	{
		public readonly GoblinUnderlingChatterType first, second, third;

		public GoblinUnderlingDialogueParticipantOrder(GoblinUnderlingChatterType first, GoblinUnderlingChatterType second, GoblinUnderlingChatterType third = GoblinUnderlingChatterType.None)
		{
			this.first = first;
			this.second = second;
			this.third = third;
		}

		public void Deconstruct(out GoblinUnderlingChatterType first, out GoblinUnderlingChatterType second, out GoblinUnderlingChatterType third)
		{
			first = this.first;
			second = this.second;
			third = this.third;
		}
	}
}
