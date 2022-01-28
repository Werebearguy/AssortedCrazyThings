using System;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Base.Data
{
    public class PetAccessoryIdentity : ModTypeIdentity, TagSerializable
    {
		public byte AltTextureIndex { get; set; }

        public PetAccessoryIdentity(string modName, string name, byte altTextureIndex) : base(modName, name)
        {
			AltTextureIndex = altTextureIndex;
		}

		public void Deconstruct(out string modName, out string name, out byte altTextureIndex)
		{
			modName = ModName;
			name = Name;
			altTextureIndex = AltTextureIndex;
		}

		public override string ToString() => base.ToString() + "; Alt: " + AltTextureIndex;

		public override TagCompound SerializeData()
		{
			var tag = base.SerializeData();
			tag.Add(nameof(AltTextureIndex), AltTextureIndex);
			return tag;
		}

		public static readonly Func<TagCompound, PetAccessoryIdentity> DESERIALIZER = Load;

		public static PetAccessoryIdentity Load(TagCompound tag)
		{
			return new PetAccessoryIdentity(tag.GetString(nameof(ModName)), tag.GetString(nameof(Name)), tag.GetByte(nameof(AltTextureIndex)));
		}
	}
}
