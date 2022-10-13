namespace AssortedCrazyThings.Projectiles
{
	/// <summary>
	/// See <seealso cref="MinionShotProj"/>. This is just a proxy for the most common aomm pet <see cref="ContentAttribute"/> to save some lines
	/// </summary>
	[Content(ContentType.AommSupport | ContentType.OtherPets)] //Give it to the base class, as it covers most aomm pets
	public abstract class MinionShotProj_AoMM : MinionShotProj
	{

	}
}
