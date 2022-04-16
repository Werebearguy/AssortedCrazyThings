using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Base.Data
{
	/// <summary>
	/// Identity classes are used for storage and easy conversion.
	/// <br>You will need "public static readonly Func<TagCompound, AModTypeIdentityClass> DESERIALIZER = Load;" in inheriting classes</br>
	/// </summary>
	public abstract class ModTypeIdentity : TagSerializable
	{
		public string ModName { get; private set; }
		public string Name { get; private set; }

		public ModTypeIdentity(string modName, string name)
		{
			ModName = modName;
			Name = name;
		}

		public override string ToString() => "ModName: " + ModName + "; Name: " + Name;

		public virtual TagCompound SerializeData()
		{
			return new TagCompound {
				{nameof(ModName), ModName },
				{nameof(Name), Name },
			};
		}
	}
}
