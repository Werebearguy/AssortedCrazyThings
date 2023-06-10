using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedCrazyThings.NPCs
{
	public class DemonEyeMechanical : DemonEyeRecolorBase
	{
		public override int TotalNumberOfThese => 3;

		/*MG = 0
        * MP = 1
        * MR = 2
        */
		public override string Texture
		{
			get
			{
				return "AssortedCrazyThings/NPCs/DemonEyeMechanical_0"; //use fixed texture
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return !NPC.downedMechBoss2 ? 0f : SpawnCondition.OverworldNightMonster.Chance * 0.025f;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				var entitySource = NPC.GetSource_Death();
				switch ((int)AiTexture)
				{
					case 0:
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
						break;
					case 1:
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
						break;
					case 2:
						Gore.NewGore(entitySource, NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeRedGore_0").Type, 1f);
						break;
					default:
						break;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/DemonEyeMechanical_" + AiTexture).Value;
			Vector2 stupidOffset = new Vector2(0f, 0f); //gfxoffY is for when the npc is on a slope or half brick
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetNPCColorTintedByBuffs(NPC.GetAlpha(drawColor)), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/DemonEyeMechanical_Glowmask").Value;
			SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
			Vector2 drawPos = NPC.position - screenPos + drawOrigin + new Vector2(0f, 0f);
			spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
		}
	}
}
