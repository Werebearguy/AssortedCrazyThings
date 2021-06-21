using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    /// <summary>
    /// Provided a pet projectile type, and a buff type, handles spawning the pet via using the item
    /// </summary>
    [Autoload]
    public abstract class SimplePetItemBase : AssItem
    {
        public abstract int PetType { get; }

        public abstract int BuffType { get; }

        public sealed override void SetDefaults()
        {
            Item.DefaultToVanitypet(PetType, BuffType);
            Item.value = 0;

            SafeSetDefaults();
        }

        public virtual void SafeSetDefaults()
        {

        }

        public sealed override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 3600, true);
            return SafeShoot(player, source, position, velocity, type, damage, knockback);
        }

        //By default, do not shoot, as the buff handles it already
        public virtual bool SafeShoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
