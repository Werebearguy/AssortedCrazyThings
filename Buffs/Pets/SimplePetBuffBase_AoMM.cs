using AssortedCrazyThings.Base.ModSupport.AoMM;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
	/// <summary>
	/// Only loaded if AoMM is enabled, handles texture, pet type, pet bool, name/tooltip, update
	/// </summary>
	[Content(ContentType.AommSupport | ContentType.OtherPets)] //Give it to the base class, as it covers most pets
	public abstract class SimplePetBuffBase_AoMM<T> : SimplePetBuffBase where T : SimplePetBuffBase
	{
		public virtual int BaseBuffType => ModContent.BuffType<T>();
		public ModBuff BaseModBuff => BuffLoader.GetBuff(BaseBuffType);

		public override int PetType => (BaseModBuff as T).PetType;

		public override ref bool PetBool(Player player) => ref (BaseModBuff as T).PetBool(player);

		public override void Update(Player player, ref int buffIndex) => BaseModBuff.Update(player, ref buffIndex);

		//Use base buff texture
		public override string Texture => BaseModBuff.Texture;

		public sealed override void SafeSetStaticDefaults()
		{
			string name = BaseModBuff.Name;
			// DisplayName.SetDefault("{$Mods.AssortedCrazyThings.BuffName." + name + "} {$Mods.AssortedCrazyThings.Common.AoMMVersion}");
			// Description.SetDefault("{$Mods.AssortedCrazyThings.BuffDescription." + name + "}");

			EvenSaferSetStaticDefaults();
		}

		public virtual void EvenSaferSetStaticDefaults()
		{

		}
	}
}
