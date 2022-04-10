using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.GameContent.UI.BigProgressBar;
using AssortedCrazyThings.NPCs.DungeonBird;

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

        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float lifePercent, ref float shieldPercent)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];
            if (!npc.active)
            {
                return false;
            }

            bossHeadIndex = npc.GetBossHeadTextureIndex();
            lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);
            shieldPercent = 0f;

            if (npc.ModNPC is Harvester harvester)
            {
                if (!harvester.IsReviving)
                {
                    return base.ModifyInfo(ref info, ref lifePercent, ref shieldPercent);
                }

                lifePercent = 0f;

                var stats = harvester.GetAIStats();
                shieldPercent = Utils.Clamp(harvester.ReviveProgress / Harvester.Revive_Duration, 0f, 1f);
                shieldPercent = Utils.Remap(shieldPercent, 0f, 1f, Harvester.Revive_MinHP, stats.MaxHP);
            }

            return true;
        }
    }
}
