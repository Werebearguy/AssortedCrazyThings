using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Weapons
{
	public class weap_laser_01 : ModItem
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Sight of Retinazer");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LaserRifle);
			item.width = 56;
			item.height = 26;
			item.damage = 40;
			item.mana = 0;
            item.shoot = ProjectileID.MiniRetinaLaser;
			item.shootSpeed = 15f;
			item.noMelee = true; 
            item.ranged = true;
			item.useTime = 10;
			item.value = 10000;
			item.rare = -11;
			item.autoReuse = true;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 0f;
			if (Collision.CanHit(position, 5, 0, position + muzzleOffset, 5, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}
	}
}