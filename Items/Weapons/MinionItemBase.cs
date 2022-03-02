using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Weapons
{
    //Custom damage scaling works as long as you use the same calculations as ModifyWeaponDamage (NOT calling GetWeaponDamage, that might have modded buffs)
    /// <summary>
    /// Item that applies a buff. Does not spawn a projectile on use by default
    /// </summary>
    [Content(ContentType.Weapons)]
    public abstract class MinionItemBase : AssItem
    {
        public sealed override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;

            SafeSetStaticDefaults();
        }


        public virtual void SafeSetStaticDefaults()
        {

        }

        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            return SafeShoot(player, source, position, velocity, type, damage, knockback);
        }

        public virtual bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
