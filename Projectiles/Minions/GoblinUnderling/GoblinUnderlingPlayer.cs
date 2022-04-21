using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderling
{
	[Content(ContentType.Weapons)]
	public class GoblinUnderlingPlayer : AssPlayerBase
	{
		private bool hasValhallaArmorVisual = false;
		private bool prevHasValhallaArmorVisual = false;

		public bool hasMinion = false;

		public bool firstSummon = true;

		public override void Load()
		{
			On.Terraria.Player.Spawn += OnSpawnSummonGoblinUnderling;
		}

		private static void OnSpawnSummonGoblinUnderling(On.Terraria.Player.orig_Spawn orig, Player player, PlayerSpawnContext context)
		{
			orig(player, context);

			if (player.whoAmI == Main.myPlayer && (context == PlayerSpawnContext.ReviveFromDeath || context == PlayerSpawnContext.SpawningIntoWorld))
			{
				if (!ClientConfig.Instance.SatchelofGoodiesAutosummon)
				{
					return;
				}

				int index = player.FindItem(ModContent.ItemType<GoblinUnderlingItem>());
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
#if TML_2022_03
					int pIndex = Projectile.NewProjectile(player.GetProjectileSource_Item(item), player.Top, Vector2.Zero, projType, item.damage, item.knockBack, player.whoAmI);
#else
					int pIndex = Projectile.NewProjectile(player.GetSource_Misc(item.Name.ToString()), player.Top, Vector2.Zero, projType, item.damage, item.knockBack, player.whoAmI);
#endif
					Main.projectile[pIndex].originalDamage = item.damage;
					player.GetModPlayer<GoblinUnderlingPlayer>().hasMinion = true;

					player.AddBuff(item.buffType, 3600, false);
				}
			}
		}

		public override void ResetEffects()
		{
			prevHasValhallaArmorVisual = hasValhallaArmorVisual;
			hasValhallaArmorVisual = false;

			hasMinion = false;
		}

		public override void OnEnterWorld(Player player)
		{
			GoblinUnderlingSystem.OnEnterWorld(player);
		}

		public override void LoadData(TagCompound tag)
		{
			firstSummon = tag.GetBool("firstSummon");
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Set("firstSummon", firstSummon);
		}

		public override void UpdateVisibleVanityAccessories()
		{
			if (Main.myPlayer != Player.whoAmI)
			{
				return;
			}

			hasValhallaArmorVisual = Player.head == 210 && Player.body == 204 && Player.legs == 152;

			if (hasValhallaArmorVisual && !prevHasValhallaArmorVisual)
			{
				foreach (var proj in GoblinUnderlingSystem.GetLocalGoblinUnderlings())
				{
					GoblinUnderlingSystem.TryCreate(proj, GoblinUnderlingMessageSource.OnValhallaArmorEquipped);
				}
			}
		}
	}
}
