using AssortedCrazyThings.Tiles;
using Terraria;

namespace AssortedCrazyThings.Buffs
{
	//Not just visual (like campfires), server needs to know it exists
	[Content(ContentType.PlaceablesFunctional)]
	public class StarRodBuff : AssBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Rod");
			Description.SetDefault("Falling Stars will land closer to you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<StarRodModPlayer>().starRodTileWorking = true;
		}
	}
}
