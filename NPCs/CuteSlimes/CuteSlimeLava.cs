using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	public class CuteSlimeLava : CuteSlimeBaseNPC
	{
		public override int CatchItem
		{
			get
			{
				return ModContent.ItemType<CuteSlimeLavaItem>();
			}
		}

		public override SpawnConditionType SpawnCondition
		{
			get
			{
				return SpawnConditionType.Hell;
			}
		}

		public override bool CannotTransformIntoShimmerOrRareVariants => true;

		public override Color DustColor => new Color(253, 121, 3, 100);

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
			});
		}

		public override bool ShouldDropGel => false;

		public override void SafeSetDefaults()
		{
			NPC.lavaImmune = true;
		}

		public override Color? GetAlpha(Color drawColor)
		{
			drawColor = Color.White * 0.78f;
			drawColor.A = 75;
			return drawColor;
		}

		public override void DrawEffects(ref Color drawColor)
		{
			int widthOffset = 12;
			Dust.NewDustDirect(NPC.position + new Vector2(widthOffset, -20), NPC.width - 2 * widthOffset, NPC.height + 20, 6).noGravity = true;
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
