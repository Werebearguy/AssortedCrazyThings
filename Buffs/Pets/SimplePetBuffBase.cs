using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Buffs.Pets
{
	/// <summary>
	/// Base class for simple pet buffs with one projectile. Defaults to a regular pet (not light pet)
	/// </summary>
	[Content(ContentType.OtherPets)] //Give it to the base class, as it covers most pets
	public abstract class SimplePetBuffBase : AssBuff
	{
		public abstract ref bool PetBool(Player player);

		public abstract int PetType { get; }

		public sealed override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;

			//Buffs are localized
			SafeSetStaticDefaults();
		}

		public virtual void SafeSetStaticDefaults()
		{

		}

		//No sealed, in case manual summoning needs to take place
		public override void Update(Player player, ref int buffIndex)
		{
			//This only works for simple one-projectile pets
			player.AssSpawnPetIfNeededAndSetTime(buffIndex, ref PetBool(player), PetType);
		}
	}
}
