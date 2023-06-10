using AssortedCrazyThings.Base;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;

namespace AssortedCrazyThings.NPCs.CuteSlimes
{
	public class CuteSlimeShimmer : CuteSlimeBaseNPC
	{
		public override int CatchItem
		{
			get
			{
				return ModContent.ItemType<CuteSlimeShimmerItem>();
			}
		}

		public override SpawnConditionType SpawnCondition
		{
			get
			{
				return SpawnConditionType.None;
			}
		}

		public override bool CannotTransformIntoShimmerOrRareVariants => true;

		public override Color DustColor => Color.Transparent;

		public override void HitEffect(NPC.HitInfo hit)
		{
			//No base call
			int count = 8;
			float scale = 1.1f;
			short type = 310;
			if (NPC.life <= 0)
			{
				scale = 1.5f;
				count = 40;
			}

			for (int i = 0; i < count; i++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, type, 2 * hit.HitDirection, -1f, 80, default(Color), scale);
				if (!Main.rand.NextBool(3))
				{
					dust.noGravity = true;
				}

				dust.velocity *= 1.5f;
				dust.velocity += NPC.velocity * 0.1f;
			}
		}

		public override void SafeSetStaticDefaults()
		{
			NPCID.Sets.ShimmerImmunity[NPC.type] = true;
		}

		public override void SafeSetDefaults()
		{
			NPC.alpha = 0;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			});
		}

		public override void DrawEffects(ref Color drawColor)
		{
			Lighting.AddLight(NPC.Center, 23);
			if ((NPC.velocity.LengthSquared() > 1f && Main.rand.NextBool(3)) || Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(NPC.Hitbox), 306);
				dust.noGravity = true;
				dust.noLightEmittence = true;
				dust.alpha = 127;
				dust.color = Main.hslToRgb(((float)Main.timeForVisualEffects / 300f + Main.rand.NextFloat() * 0.1f) % 1f, 1f, 0.65f);
				dust.color.A = 0;
				dust.velocity = dust.position - NPC.Center;
				dust.velocity *= 0.1f;
				dust.velocity.X *= 0.25f;
				if (dust.velocity.Y > 0f)
				{
					dust.velocity.Y *= -1f;
				}

				dust.scale = Main.rand.NextFloat() * 0.3f + 0.5f;
				dust.fadeIn = 0.9f;
			}
		}

		public override bool SafePreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!NPC.IsABestiaryIconDummy)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
			}

			SpriteEffects spriteEffects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			float yUnused = 0f;
			float yOff = Main.NPCAddHeight(NPC);
			Vector2 halfSize = new Vector2(TextureAssets.Npc[NPC.type].Width() / 2, TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2);

			DrawData data = new DrawData(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.position.X - screenPos.X + NPC.width / 2 - TextureAssets.Npc[NPC.type].Width() * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.position.Y - screenPos.Y + NPC.height - TextureAssets.Npc[NPC.type].Height() * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + halfSize.Y * NPC.scale + yOff + yUnused + NPC.gfxOffY), NPC.frame, NPC.GetNPCColorTintedByBuffs(NPC.GetAlpha(drawColor)), NPC.rotation, halfSize, NPC.scale, spriteEffects);
			GameShaders.Misc["RainbowTownSlime"].Apply(data);
			data.Draw(spriteBatch);
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			if (!NPC.IsABestiaryIconDummy)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			}

			return false;
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
