using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs.Pets
{
    /// <summary>
    /// Base class for simple pet buffs with one projectile. Defaults to a regular pet (not light pet)
    /// </summary>
    public abstract class SimplePetBuffBase : ModBuff
    {
        public abstract ref bool PetBool(Player player);

        public abstract int PetType { get; }

        public sealed override void SetDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;

            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults()
        {

        }

        //No sealed, in case manual summoning needs to take place
        public override void Update(Player player, ref int buffIndex)
        {
            //This only works for simple one-projectile pets
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref PetBool(player), PetType);
        }
    }
}
