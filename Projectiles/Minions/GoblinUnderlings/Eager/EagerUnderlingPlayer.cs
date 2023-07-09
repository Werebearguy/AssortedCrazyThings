using AssortedCrazyThings.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings.Projectiles.Minions.GoblinUnderlings.Eager
{
	//TODO goblin repurpose for all underlings
	[Content(ContentType.Weapons)]
	public class EagerUnderlingPlayer : AssPlayerBase
	{
		public bool hasMinion = false;

		public bool firstSummon = true;

		public override void Load()
		{
			On_Player.Spawn += OnSpawnSummonGoblinUnderling;
		}

		private static void OnSpawnSummonGoblinUnderling(On_Player.orig_Spawn orig, Player player, PlayerSpawnContext context)
		{
			orig(player, context);

			if (player.whoAmI == Main.myPlayer && (context == PlayerSpawnContext.ReviveFromDeath || context == PlayerSpawnContext.SpawningIntoWorld))
			{
				if (!ClientConfig.Instance.SatchelofGoodiesAutosummon)
				{
					return;
				}

				int index = player.FindItem(ModContent.ItemType<EagerUnderlingItem>());
				if (index != -1)
				{
					Item item = player.inventory[index];
					player.AddBuff(item.buffType, 3600, false);
				}
			}
		}

		public override void ResetEffects()
		{
			hasMinion = false;
		}

		public override void LoadData(TagCompound tag)
		{
			firstSummon = tag.GetBool("firstSummon");
		}

		public override void SaveData(TagCompound tag)
		{
			tag["firstSummon"] = firstSummon;
		}
	}
}
