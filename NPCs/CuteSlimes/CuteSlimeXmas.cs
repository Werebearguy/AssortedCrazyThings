using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	public class CuteSlimeXmas : CuteSlimeBaseNPC
	{
		public override int CatchItem
		{
			get
			{
				return ModContent.ItemType<CuteSlimeXmasItem>();
			}
		}

		public override SpawnConditionType SpawnCondition
		{
			get
			{
				return SpawnConditionType.Xmas;
			}
		}

		public override bool CannotTransformIntoShimmerOrRareVariants => true;

		public override Color DustColor => new Color(115, 113, 62, 100);

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			});
		}

		public override void SafeModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.GiantBow, chanceDenominator: 5));
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture + "Addition").Value;
			Vector2 stupidOffset = new Vector2(0f, -6 * NPC.scale + NPC.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			drawColor.A = 255;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
		}
	}
}
