using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	public class CuteSlimeRainbow : CuteSlimeBaseNPC
	{
		public override int CatchItem
		{
			get
			{
				return ModContent.ItemType<CuteSlimeRainbowItem>();
			}
		}

		public override SpawnConditionType SpawnCondition
		{
			get
			{
				return SpawnConditionType.Forest;
			}
		}

		public override Color DustColor => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 100);

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("The caring and motherly nature that this slime exhibits shows that it understands compassion.")
			});
		}

		public override void SafeSetDefaults()
		{
			NPC.scale = 1.2f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			lightColor = Main.DiscoColor;
			lightColor *= (255f - NPC.alpha) / 255f;
			return lightColor;
		}
	}
}
