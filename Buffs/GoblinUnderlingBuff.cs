using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace AssortedCrazyThings.Buffs
{
	[Content(ContentType.Weapons)]
	public abstract class GoblinUnderlingBuff : AssBuff
	{
		public static LocalizedText CommonDescriptionText { get; private set; }

		public override LocalizedText Description => CommonDescriptionText;

		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;

			string category = $"Common.Tooltips.";
			CommonDescriptionText ??= Mod.GetLocalization($"{category}{nameof(GoblinUnderlingBuff)}.CommonDescription");
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
			//Minion slot check (and assignment below) is for when more than 1 is spawned in same tick, causing the projectiles to exist for 1 tick, triggering chatter
			//But it also doesnt work because on spawn players maxMinions is 1, it checks gear a tick later so there will only be 1 spawned
			//this check is important when there are other minions summoned and this one is unsummoned, it will keep resummoning itself from the buff and being active for 1 tick (doing AI)
			//So current workaround is to let summons be spawned regardless of check the moment the player spawns (armor/accessory slot increases don't apply here yet), otherwise adhere to limits
			else if (Main.myPlayer == player.whoAmI && (!modPlayer.spawned || player.numMinions < player.maxMinions))
			{
				int index = player.FindItemInInventoryOrOpenVoidBag(GoblinUnderlingItem.BuffToItem[Type], out bool voidBag);
				if (index != -1)
				{
					Item item = (!voidBag ? player.inventory : player.bank4.item)[index];
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
					Projectile goblin = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), player.Top, Vector2.Zero, projType, item.damage, item.knockBack, player.whoAmI);
					goblin.originalDamage = item.damage;
					//player.slotsMinions += goblin.minionSlots;
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
