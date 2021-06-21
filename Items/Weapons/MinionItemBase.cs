using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace AssortedCrazyThings.Items.Weapons
{
    [Autoload]
    public abstract class MinionItemBase : AssItem
    {
        public sealed override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            return SafeShoot(player, source, position, velocity, type, damage, knockback);
        }

        public virtual bool SafeShoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
    }
}
