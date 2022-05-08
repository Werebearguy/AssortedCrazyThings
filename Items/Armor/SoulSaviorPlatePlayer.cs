using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Armor
{
	[Content(ContentType.Bosses)]
	public class SoulSaviorPlatePlayer : AssPlayerBase
	{
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			int bodyType = EquipLoader.GetEquipSlot(Mod, nameof(SoulSaviorPlate), EquipType.Body);
			if (drawInfo.drawPlayer.body == bodyType)
			{
				drawInfo.bodyGlowColor = Color.White;
			}
		}
	}
}
