using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Weapons)]
	public class EagerUnderlingBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			bool active = true;
			EagerUnderlingPlayer modPlayer = player.GetModPlayer<EagerUnderlingPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EagerUnderlingProj>()] > 0)
			{
				modPlayer.hasMinion = true;
			}
			else if (Main.myPlayer == player.whoAmI && player.numMinions < player.maxMinions)
			{
				int index = player.FindItem(ModContent.ItemType<EagerUnderlingItem>());
				if (index != -1)
				{
					Item item = player.inventory[index];

					int projType = item.shoot;
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
					modPlayer.hasMinion = true;
				}
			}

			if (!modPlayer.hasMinion)
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
