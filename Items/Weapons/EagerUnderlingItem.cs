using AssortedCrazyThings.Base;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings;
using AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
	[LegacyName("GoblinUnderlingItem")]
	[Content(ContentType.Weapons)]
	public class EagerUnderlingItem : MinionItemBase
	{
		public const int BaseDmg = 8;
		public const float BaseKB = 1.5f;

		public override void Load()
		{
			On_NPC.SetEventFlagCleared += DropItemIfPossible;
		}

		private static void DropItemIfPossible(On_NPC.orig_SetEventFlagCleared orig, ref bool eventFlag, int gameEventId)
		{
			//This is not clientside
			orig(ref eventFlag, gameEventId);

			if (gameEventId == GameEventClearedID.DefeatedGoblinArmy)
			{
				int itemType = ModContent.ItemType<EagerUnderlingItem>();

				static bool Condition(Player player, int itemType) => !player.HasItemWithBanks(itemType);

				if (Main.netMode == NetmodeID.Server)
				{
					int itemIndex = -1;
					for (int p = 0; p < Main.maxPlayers; p++)
					{
						Player player = Main.player[p];
						if (player.active)
						{
							if (Condition(player, itemType))
							{
								int item = Item.NewItem(new EntitySource_WorldEvent(), player.Center, itemType, noBroadcast: true);
								itemIndex = item;
								NetMessage.SendData(MessageID.InstancedItem, p, -1, null, item);
								Main.item[item].active = false;
							}
						}
					}

					if (itemIndex != -1)
					{
						Main.timeItemSlotCannotBeReusedFor[itemIndex] = 54000;
					}
				}
				else if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Player player = Main.LocalPlayer;
					if (Condition(player, itemType))
					{
						Item.NewItem(new EntitySource_WorldEvent(), player.Center, itemType);
					}
				}
			}
		}

		public override void SetDefaults()
		{
			Item.damage = BaseDmg;
			Item.knockBack = BaseKB;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 2;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ModContent.ProjectileType<EagerUnderlingProj>();
			Item.shootSpeed = 0f;
			Item.buffType = ModContent.BuffType<EagerUnderlingBuff>();
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			//This is purely done for the tooltip
			damage *= GoblinUnderlingTierSystem.GetCurrentTierStats(Item.shoot).damageMult;
		}

		public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
		{
			//This is purely done for the tooltip
			knockback *= GoblinUnderlingTierSystem.GetCurrentTierStats(Item.shoot).knockbackMult;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.ownedProjectileCounts[type] > 0)
			{
				//Use always resummons
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile other = Main.projectile[i];
					if (other.active && other.owner == player.whoAmI && other.type == type)
					{
						other.Kill();
					}
				}
			}

			knockback = Item.knockBack; //Use baseline values
			int origDamage = Item.damage;

			int index = Projectile.NewProjectile(source, position, Vector2.UnitX * player.direction, type, damage, knockback, Main.myPlayer);
			Main.projectile[index].originalDamage = origDamage;
			return false;
		}
	}
}