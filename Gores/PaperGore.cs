using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Gores
{
	public class PaperGore : ModGore
	{
		public override void SetStaticDefaults()
		{
			GoreID.Sets.DisappearSpeedAlpha[Type] = 10;
			GoreID.Sets.DisappearSpeed[Type] = 6;
			ChildSafety.SafeGore[Type] = true;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.Frame = new SpriteFrame(1, 8, 0, (byte)Main.rand.Next(8));
			gore.frameCounter = (byte)Main.rand.Next(8);
			gore.timeLeft = 120;
		}

		public override bool Update(Gore gore)
		{
			if (++gore.frameCounter >= 8 && gore.velocity.Y > 0.2f)
			{
				gore.frameCounter = 0;
				int num14 = gore.Frame.CurrentRow / 4;
				if (++gore.Frame.CurrentRow >= 4 + num14 * 4)
					gore.Frame.CurrentRow = (byte)(num14 * 4);
			}

			PaperScrapGore.Gore_UpdateLeaf(gore, 0.2f, false);

			return false;
		}
	}
}
