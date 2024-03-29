using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Weapons
{
	//Custom damage scaling works as long as you use the same calculations as ModifyWeaponDamage (NOT calling GetWeaponDamage, that might have modded buffs)
	/// <summary>
	/// Item that applies a buff. Does not spawn a projectile on use by default
	/// </summary>
	public abstract class MinionItemBase : WeaponItemBase
	{
		public sealed override void SafeSetStaticDefaults()
		{
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;

			EvenSaferSetStaticDefaults();
		}

		public virtual void EvenSaferSetStaticDefaults()
		{

		}

		public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (!(player.altFunctionUse == 2 && ProjectileID.Sets.MinionTargettingFeature[Item.shoot]))
			{
				player.AddBuff(Item.buffType, 2);
			}

			return SafeShoot(player, source, position, velocity, type, damage, knockback);
		}

		public virtual bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}
}
