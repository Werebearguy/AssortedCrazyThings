using AssortedCrazyThings.NPCs.Harvester;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;

namespace AssortedCrazyThings.BossBars
{
	//Special health bar because the revive stage needs progression representation
	public class HarvesterBossBar : ModBossBar
	{
		private int bossHeadIndex = -1;

		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			if (bossHeadIndex != -1)
			{
				return TextureAssets.NpcHeadBoss[bossHeadIndex];
			}
			return null;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
		{
			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
			{
				return false;
			}

			bossHeadIndex = npc.GetBossHeadTextureIndex();
			life = Utils.Clamp(npc.life, 0f, lifeMax);
			shield = 0f;

			if (npc.ModNPC is HarvesterBoss harvester)
			{
				if (!harvester.IsReviving)
				{
					return base.ModifyInfo(ref info, ref life, ref lifeMax, ref shield, ref shieldMax);
				}

				life = 0f;

				var stats = harvester.GetAIStats();
				shieldMax = lifeMax;
				shield = Utils.Clamp(harvester.ReviveProgress / HarvesterBoss.Revive_Duration, 0f, 1f);
				shield = shieldMax * Utils.Remap(shield, 0f, 1f, HarvesterBoss.Revive_MinHP, stats.MaxHP);
			}

			return true;
		}
	}
}
