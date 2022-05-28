using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.HostileNPCs)] //Important
	public class RoyalGelGlobalItem : AssGlobalItem
	{
		/// <summary>
		/// Populated by the NPCs themselves, includes all NPCs that should be concidered towards player.npcTypeNoAggro when wearing the Royal Gel accessory
		/// </summary>
		internal static HashSet<int> RoyalGelNoAggroNPCs;

		public override void Load()
		{
			RoyalGelNoAggroNPCs = new HashSet<int>();
		}

		public override void Unload()
		{
			RoyalGelNoAggroNPCs = null;
		}

		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return lateInstantiation && entity.type == ItemID.RoyalGel;
		}

		public override void UpdateEquip(Item item, Player player)
		{
			foreach (var npcType in RoyalGelNoAggroNPCs)
			{
				player.npcTypeNoAggro[npcType] = true;
			}
		}
	}
}
