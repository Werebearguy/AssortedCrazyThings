using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	public class CuteSlimeIlluminant : CuteSlimeBaseNPC
	{
		public override int CatchItem
		{
			get
			{
				return ModContent.ItemType<CuteSlimeIlluminantItem>();
			}
		}

		public override SpawnConditionType SpawnCondition
		{
			get
			{
				return SpawnConditionType.Hallow;
			}
		}

		public override Color DustColor => new Color(202, 59, 231, 100);

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow,
				new FlavorTextBestiaryInfoElement("The erratic nature of the Hallow has made this slime quite playful.")
			});
		}

		public override void SafeSetStaticDefaults()
		{
			NPCID.Sets.TrailingMode[NPC.type] = 3;
			NPCID.Sets.TrailCacheLength[NPC.type] = 8;
		}

		public override void SafeSetDefaults()
		{
			DrawOffsetY = 1f;
			NPC.alpha = 80;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/CuteSlimes/CuteSlimeIlluminantAddition").Value;
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2 + NPC.gfxOffY - 5f);

			// The higher the k, the older the position
			// Length is implicitely set in TrailCacheLength up there
			for (int k = NPC.oldPos.Length - 1; k >= 0; k--)
			{
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin;
				Color color = NPC.GetAlpha(Color.White) * ((NPC.oldPos.Length - k) / (1f * NPC.oldPos.Length)) * ((255 - NPC.alpha) / 255f) * 0.5f;
				color.A = (byte)(NPC.alpha * ((NPC.oldPos.Length - k) / NPC.oldPos.Length));
				spriteBatch.Draw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			}
		}
	}
}
