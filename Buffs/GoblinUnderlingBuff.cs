using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using Microsoft.Xna.Framework;
using Terraria;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			bool active = true;
			GoblinUnderlingPlayer modPlayer = player.GetModPlayer<GoblinUnderlingPlayer>();
			int projType = GoblinUnderlingItem.BuffToProjectile[Type];
			if (player.ownedProjectileCounts[projType] > 0)
			{
				modPlayer.SetHasMinion(projType, true);
			}
			else if (Main.myPlayer == player.whoAmI && player.numMinions < player.maxMinions)
			{
				int index = player.FindItem(GoblinUnderlingItem.BuffToItem[Type]);
				if (index != -1)
				{
					Item item = player.inventory[index];

					projType = item.shoot;
					if (player.ownedProjectileCounts[projType] > 0)
					{
						//Mostly failsafe, but if minion still alive, kill it, to avoid duplicate
						for (int i = 0; i < Main.maxProjectiles; i++)
						{
							Projectile other = Main.projectile[i];
							if (other.active && other.owner == player.whoAmI && other.type == projType)
							{
								other.Kill();
							}
						}
					}
					//Source is important so that it applies the selected class to the minion
					int pIndex = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Top, Vector2.Zero, projType, item.damage, item.knockBack, player.whoAmI);
					Main.projectile[pIndex].originalDamage = item.damage;
					modPlayer.SetHasMinion(projType, true);
				}
			}

			if (!modPlayer.GetHasMinion(projType))
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				active = false;
			}

			if (active)
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
